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
using task_service.API.Middleware;
using task_service.Application.Behaviors;
using task_service.Application.Validation;
using task_service.Domain.Interfaces;
using task_service.Infrastructure.Configuration;
using task_service.Infrastructure.Data;
using task_service.Infrastructure.Ftp;
using task_service.Infrastructure.Persistence;
using task_service.Infrastructure.Services;


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
static bool ShouldLog(LogEvent logEvent)
{
    // Registrar solo logs que contengan la palabra clave "definido por el usuario"
    return logEvent.MessageTemplate.Text.Contains("more2286");
}

// Reemplazar el logger predeterminado por Serilog
builder.Host.UseSerilog();

// Configuración FtpSettings desde appsettings.json o docker-compose
builder.Services.Configure<FtpSettings>(builder.Configuration.GetSection("FtpSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<FtpSettings>>().Value);

// Configuración RabbitMQ desde appsettings.json o docker-compose
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);

// Configuración ClientSettings desde appsettings.json o docker-compose
builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));
builder.Services.AddHttpClient<IUsuarioService, UsuarioServicio>();

// Inyección de dependencias
builder.Services.AddSingleton<FtpHelper>();
builder.Services.AddHostedService<FtpTareaProcesarServicio>();
builder.Services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisherServicio>();

builder.Services.AddControllers();

// Agregamos los servicios de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskService API", Version = "v1" });
});
// Configuracion para AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// ✅ Registrar FluentValidation (busca validadores en todo el ensamblado)
builder.Services.AddValidatorsFromAssembly(typeof(CreateTareaCommandValidator).Assembly);

// ✅ Registrar el pipeline de validación para todos los Commands
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Configuracion para MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

// Configuracion para la inyeccion de dependencias
builder.Services.AddScoped<IConnectionFactory, DbConnectionSqlServer>();
builder.Services.AddScoped<IDbConnection>(sp => {
    var factory = sp.GetRequiredService<IConnectionFactory>();
    return factory.CreateConnection();
});

// Registro de repositorios
builder.Services.AddScoped<ITareaRepositorio, TareaRepositorio>();

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

// <-- esto al final expone la clase Program
public partial class Program { }