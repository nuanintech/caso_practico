using MediatR;
using task_service.Domain.Entities;
using task_service.Domain.Enum;

namespace task_service.Application.Commands {
    public class CreateTareaCommand : IRequest<Tarea> {
        public string CodigoTarea { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? CriteriosAceptacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public StateTask EstadoTarea { get; set; } = StateTask.Backlog; // Backlog, Doing, InReview, Done
        public State Estado { get; set; } = State.Activo;       // Activo, Inactivo
        public Guid? UsuarioId { get; set; }
    }
}
