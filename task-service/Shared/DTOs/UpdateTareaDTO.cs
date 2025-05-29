namespace task_service.Shared.DTOs
{
    public class UpdateTareaDTO {
        public string CodigoTarea { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? CriteriosAceptacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
