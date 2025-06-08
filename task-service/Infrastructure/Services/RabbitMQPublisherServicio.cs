using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using task_service.Domain.Interfaces;
using task_service.Infrastructure.Configuration;
using task_service.Shared.DTOs;

namespace task_service.Infrastructure.Services
{
    public class RabbitMQPublisherServicio : IRabbitMQPublisher {
        private readonly ILogger<RabbitMQPublisherServicio> logger;
        private readonly RabbitMQSettings rabbitMQSettings;
        private readonly ConnectionFactory factory;
        private readonly string queueName;
        public RabbitMQPublisherServicio(IConfiguration configuration, ILogger<RabbitMQPublisherServicio> logger, IOptions<RabbitMQSettings> rabbitMQOptions) {
            this.logger = logger;
            this.rabbitMQSettings = rabbitMQOptions.Value;
            factory = new ConnectionFactory {
                HostName = rabbitMQSettings.Host ?? "rabbitmq",
                Port = rabbitMQSettings.Puerto > 0 ? rabbitMQSettings.Puerto : 5672, // Puerto por defecto de RabbitMQ
                UserName = rabbitMQSettings.Usuario,
                Password = rabbitMQSettings.Contrasena
            };
            queueName = rabbitMQSettings.TareaAsignadaQueue ?? "task_assigned";
        }

        public void PublishTaskAssigned(MessageDTO messageDTO) {
            try {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var json = JsonConvert.SerializeObject(messageDTO);
                var body = Encoding.UTF8.GetBytes(json);
                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: props,
                                     body: body);
                logger.LogInformation("Mensaje de asignación publicado para la tarea {Codigo}", messageDTO.TareaId);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
