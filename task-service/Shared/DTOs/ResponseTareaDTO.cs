namespace task_service.Shared.DTOs {
    public class ResponseTareaDTO {
        public Guid Id { get; set; }
        public string CodigoTarea { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? CriteriosAceptacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int? TiempoDias { get; set; }
        public string EstadoTarea { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public Guid? UsuarioId { get; set; }
    }
}
