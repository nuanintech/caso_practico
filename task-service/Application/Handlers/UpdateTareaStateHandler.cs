using MediatR;
using task_service.Application.Commands;
using task_service.Domain.Enum;
using task_service.Domain.Exceptions;
using task_service.Domain.Interfaces;

namespace task_service.Application.Handlers
{
    public class UpdateTareaStateHandler : IRequestHandler<UpdateTareaStateCommand, Unit> {
        private readonly ITareaRepositorio tareaRepositorio;

        public UpdateTareaStateHandler(ITareaRepositorio repositorio) {
            tareaRepositorio = repositorio;
        }

        public async Task<Unit> Handle(UpdateTareaStateCommand request, CancellationToken cancellationToken) {
            var tarea = await tareaRepositorio.GetByIdAsync(request.Id);
            if (tarea is null)
                throw new NotFoundException($"No se encontró la tarea con Id : {request.Id}");

            await tareaRepositorio.UpdateStateAsync(request.Id, request.updateStateDTO.EstadoTarea);
            return Unit.Value;
        }
    }
}
