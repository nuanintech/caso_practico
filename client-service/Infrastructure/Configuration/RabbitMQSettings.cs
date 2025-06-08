namespace client_service.Infrastructure.Configuration
{
    public class RabbitMQSettings {
        public string Host { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string TareaAsignadaQueue { get; set; } = string.Empty;
        public int Puerto { get; set; } = 5672; // Puerto por defecto de RabbitMQ
    }
}
