using MediatR;
using task_service.Application.Queries;
using task_service.Domain.Entities;
using task_service.Domain.Interfaces;

namespace task_service.Application.Handlers {
    public class GetAllTareasHandler : IRequestHandler<GetAllTareasQuery, IEnumerable<Tarea>> {
        private readonly ITareaRepositorio tareaRepositorio;

        public GetAllTareasHandler(ITareaRepositorio repositorio) {
            this.tareaRepositorio = repositorio;
        }

        public async Task<IEnumerable<Tarea>> Handle(GetAllTareasQuery request, CancellationToken cancellationToken) {
            return await tareaRepositorio.GetAllAsync();
        }
    }
}
