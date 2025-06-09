using MediatR;
using task_service.Application.Commands;
using task_service.Domain.Exceptions;
using task_service.Domain.Interfaces;

namespace task_service.Application.Handlers {
    public class DeleteTareaHandler : IRequestHandler<DeleteTareaCommand, Unit> {
        private readonly ITareaRepositorio tareaRepositorio;

        public DeleteTareaHandler(ITareaRepositorio repositorio) {
            tareaRepositorio = repositorio;
        }

        public async Task<Unit> Handle(DeleteTareaCommand request, CancellationToken cancellationToken) {
            var tarea = await tareaRepositorio.GetByIdAsync(request.Id);
            if (tarea is null)
                throw new NotFoundException($"No se encontró la tarea con Id : {request.Id}");

            await tareaRepositorio.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}

