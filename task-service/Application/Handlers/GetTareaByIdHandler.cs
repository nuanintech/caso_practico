using MediatR;
using task_service.Application.Queries;
using task_service.Domain.Entities;
using task_service.Domain.Exceptions;
using task_service.Domain.Interfaces;

namespace task_service.Application.Handlers {
    public class GetTareaByIdHandler : IRequestHandler<GetTareaByIdQuery, Tarea?> {
        private readonly ITareaRepositorio tareaRepositorio;

        public GetTareaByIdHandler(ITareaRepositorio repositorio) {
            tareaRepositorio = repositorio;
        }

        public async Task<Tarea?> Handle(GetTareaByIdQuery request, CancellationToken cancellationToken) {
            var tarea = await tareaRepositorio.GetByIdAsync(request.Id);
            if (tarea is null)
                throw new NotFoundException($"No se encontró la tarea con Id : {request.Id}");

            return await tareaRepositorio.GetByIdAsync(request.Id);
        }
    }
}
