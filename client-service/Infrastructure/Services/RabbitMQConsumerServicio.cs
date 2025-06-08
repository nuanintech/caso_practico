using client_service.Domain.Interfaces;
using client_service.Infrastructure.Configuration;
using client_service.Infrastructure.Data;
using client_service.Shared.DTOs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace client_service.Infrastructure.Services
{
    public class RabbitMQConsumerServicio : BackgroundService {
        private readonly ILogger<RabbitMQConsumerServicio> logger;
        private readonly RabbitMQSettings rabbitMQSettings;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IEmailService emailService; // Asumiendo que tienes un servicio de email para enviar notificaciones
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string queueName;

        public RabbitMQConsumerServicio(IConfiguration configuration, ILogger<RabbitMQConsumerServicio> logger, IOptions<RabbitMQSettings> rabbitMQOptions, IEmailService emailService, IServiceScopeFactory scopeFactory) {
            this.logger = logger;
            this.rabbitMQSettings = rabbitMQOptions.Value;
            this.emailService = emailService; 
            this.serviceScopeFactory = scopeFactory;
            var factory = new ConnectionFactory {
                HostName = rabbitMQSettings.Host ?? "rabbitmq",
                Port = rabbitMQSettings.Puerto > 0 ? rabbitMQSettings.Puerto : 5672, // Puerto por defecto de RabbitMQ
                UserName = rabbitMQSettings.Usuario,
                Password = rabbitMQSettings.Contrasena
            };
            queueName = rabbitMQSettings.TareaAsignadaQueue ?? "task_assigned";
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) => {
                using var scope = serviceScopeFactory.CreateScope();
                var usuarioRepositorio = scope.ServiceProvider.GetRequiredService<IUsuarioRepositorio>();
                
                try {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var message = JsonConvert.DeserializeObject<MessageDTO>(json);

                    if (message == null)
                        return;

                    // Recuperar usuario de forma asincrónica
                    var usuario = await usuarioRepositorio.GetByIdAsync(message.UsuarioId);
                    if (usuario == null) {
                        logger.LogWarning("more2286: Usuario con ID {UsuarioId} no encontrado, omitiendo notificación.", message.UsuarioId);
                        return;
                    }

                    // 1) Loguear
                    logger.LogInformation("more2286: Tarea {Codigo} asignada al usuario {Usuario}", message.CodigoTarea, message.UsuarioId);

                    // 2) Enviar correo
                    var subject = $"Nueva tarea asignada: {message.CodigoTarea}";
                    var bodyHtml = $@"
                    <p>Hola,</p>
                    <p>Se te ha asignado la tarea <strong>{message.CodigoTarea}</strong>:</p>
                    <ul>
                        <li><strong>Título:</strong> {message.Titulo}</li>
                    </ul>
                    <p>Saludos,<br/>Tu sistema de tareas</p>";

                
                    await emailService.SendEmailAsync(usuario.Email, subject, bodyHtml);
                    logger.LogInformation("more2286: Correo enviado a {Email} para la tarea {Codigo}", usuario.Email, message.CodigoTarea);
                }
                catch (Exception ex) {
                    logger.LogError(ex,"more2286: Error procesando mensaje de tarea asignada: {Error}",ex.Message);
                }
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            return Task.CompletedTask;
        }

        public override void Dispose() {
            channel.Close();
            connection.Close();
            base.Dispose();
        }
    }
}
