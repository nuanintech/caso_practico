using client_service.API.Middleware;
using client_service.Domain.Interfaces;
using client_service.Infrastructure.Configuration;
using client_service.Infrastructure.Data;
using client_service.Infrastructure.Persistence;
using client_service.Infrastructure.Services;
using client_service.Shared.Validator;
using client_service.Shared.Validator.FilterValidator;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Cambia la ruta del archivo de logs a un directorio accesible y persistente dentro del contenedor Docker.
// Por convención, usa "/app/Logs" o una ruta configurable por variable de entorno.
var logDirectory = Environment.GetEnvironmentVariable("LOG_DIR")
                   ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

// Asegúrate de que el directorio exista antes de inicializar el logger.
Directory.CreateDirectory(logDirectory);
var logFilePath = Path.Combine(logDirectory, "log-.txt");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
       logFilePath, // Ruta absoluta
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information, // Nivel mínimo de logs
        outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .Filter.ByIncludingOnly(logEvent => ShouldLog(logEvent)) // Filtrar los eventos específicos
    .CreateLogger();

// Función para determinar qué eventos deben ser registrados
static bool ShouldLog(LogEvent logEvent) {
    // Registrar solo logs que contengan la palabra clave "definido por el usuario"
    return logEvent.MessageTemplate.Text.Contains("more2286");
}

// Reemplazar el logger predeterminado por Serilog
builder.Host.UseSerilog();

// Cambia la línea problemática para evitar el uso de la propiedad inexistente "SuppressModelStateInvalidFilter".
// En su lugar, utiliza un filtro global para deshabilitar la validación automática del estado del modelo.

builder.Services.AddControllers(options => {
    options.Filters.Add(typeof(ModelStateValidationFilter)); 
}).ConfigureApiBehaviorOptions(options => {
    options.SuppressModelStateInvalidFilter = true; // Deshabilitación de la validación automática del estado del modelo
});
// Registro del filtro personalizado
builder.Services.AddScoped<ModelStateValidationFilter>();

// Obtiene todos los validadores de DTOs y se basa en la convención de nombres para registrarlos automáticamente
builder.Services.AddValidatorsFromAssembly(typeof(CreateUsuarioDTOValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();

// Configuración RabbitMQ desde appsettings.json o docker-compose
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);

// Configuración EmailSettings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);
builder.Services.AddTransient<IEmailService, EmailServicio>();

builder.Services.AddHostedService<RabbitMQConsumerServicio>(); // Registro del consumidor de RabbitMQ

// Agregamos los servicios de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClientService API", Version = "v1" });
});
// Configuracion para AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
// Configuracion para MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

// Configuracion para la inyeccion de dependencias
builder.Services.AddScoped<IConnectionFactory, DbConnectionMySql>();
builder.Services.AddScoped<IDbConnection>(sp => {
    var factory = sp.GetRequiredService<IConnectionFactory>();
    return factory.CreateConnection();
});

// Registro de repositorios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//  Middleware de manejo de errores (debe ir antes de cualquier lógica de autorización o endpoint)
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();