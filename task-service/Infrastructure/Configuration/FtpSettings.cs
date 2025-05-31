namespace task_service.Infrastructure.Configuration
{
    public class FtpSettings {
        public string Host { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string RutaPendientes { get; set; } = "pendientes/";
        public string RutaProcesados { get; set; } = "procesados/";
        public string RutaErrores { get; set; } = "errores/";
        public int IntervaloMinutos { get; set; } = 5;
    }
}
