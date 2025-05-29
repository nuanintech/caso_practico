namespace client_service.Shared.DTOs
{
    public class UpdateUsuarioDTO {
        public string Identificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int? Edad { get; set; }
        public string? Cargo { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
