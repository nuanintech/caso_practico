namespace client_service.Infrastructure.Configuration
{
    public class EmailSettings {
        public string Host { get; set; } = string.Empty;
        public int Puerto { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public bool EnableSsl { get; set; } = true;
    }
}
