namespace client_service.Shared.DTOs {
    public class ResponseUsuarioDTO {
        public Guid Id { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Edad { get; set; }     // VO convertido a int
        public string? Cargo { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}

