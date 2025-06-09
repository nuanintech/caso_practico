using MediatR;
using task_service.Domain.Entities;

namespace task_service.Application.Queries {
    public class GetTareasByUsuarioIdQuery : IRequest<IEnumerable<Tarea>> {
        public Guid UsuarioId { get; }

        public GetTareasByUsuarioIdQuery(Guid usuarioId) {
            UsuarioId = usuarioId;
        }
    }
}

