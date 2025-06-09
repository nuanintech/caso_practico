using MediatR;
using task_service.Application.Queries;
using task_service.Domain.Entities;
using task_service.Domain.Interfaces;

namespace task_service.Application.Handlers {
    public class GetTareasByUsuarioIdHandler : IRequestHandler<GetTareasByUsuarioIdQuery, IEnumerable<Tarea>> {
        private readonly ITareaRepositorio tareaRepositorio;

        public GetTareasByUsuarioIdHandler(ITareaRepositorio repositorio) {
            tareaRepositorio = repositorio;
        }

        public async Task<IEnumerable<Tarea>> Handle(GetTareasByUsuarioIdQuery request, CancellationToken cancellationToken) {
            return await tareaRepositorio.GetByUsuarioIdAsync(request.UsuarioId);
        }
    }
}