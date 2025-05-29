namespace task_service.Shared.DTOs {
    public class CreateTareaDTO {
        public string CodigoTarea { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? CriteriosAceptacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string EstadoTarea { get; set; } = "Backlog";
        public Guid? UsuarioId { get; set; }
    }
}
