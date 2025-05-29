using task_service.Domain.Enum;

namespace task_service.Domain.Entities {
    public class Tarea {
        public Guid Id { get; set; }
        public required string CodigoTarea { get; set; }
        public required string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? CriteriosAceptacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? TiempoDias { get; set; }

        // Estados válidos: Backlog, Doing, In Review, Done
        public string EstadoTarea { get; set; } = "Backlog";

        // Estados válidos: Activo, Inactivo
        public string Estado { get; set; } = "Activo";

        public Guid? UsuarioId { get; set; }
    }
}
