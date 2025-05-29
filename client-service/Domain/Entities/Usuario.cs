namespace client_service.Domain.Entities
{
    public class Usuario {
        public Guid Id { get; set; }
        public required string Identificacion { get; set; }
        public required string Nombres { get; set; }
        public required string Apellidos { get; set; }
        public int? Edad { get; set; }
        public required string Email { get; set; }
        public string? Cargo { get; set; }

        // Estados válidos: Activo, Inactivo
        public required string Estado { get; set; }
    }
}
