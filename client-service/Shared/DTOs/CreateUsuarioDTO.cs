namespace client_service.Shared.DTOs {
    public class CreateUsuarioDTO {
        public string Identificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? Edad { get; set; }
        public string? Cargo { get; set; }
    }
}
