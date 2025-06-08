using MediatR;
using task_service.Application.Commands;
using task_service.Domain.Exceptions;
using task_service.Domain.Interfaces;
using task_service.Infrastructure.Services;
using task_service.Shared.DTOs;

namespace task_service.Application.Handlers
{
    public class AssignTareaHandler : IRequestHandler<AssignTareaCommand, Unit> {
        private readonly ITareaRepositorio tareaRepositorio;
        private readonly IUsuarioService usuarioServicio;
        private readonly IRabbitMQPublisher rabbitMQPublisher;

        public AssignTareaHandler(ITareaRepositorio tareaRepositorio, IUsuarioService usuarioServicio, IRabbitMQPublisher rabbitMQPublisher) {
            this.tareaRepositorio = tareaRepositorio;
            this.usuarioServicio = usuarioServicio;
            this.rabbitMQPublisher = rabbitMQPublisher;
        }
        public async Task<Unit> Handle(AssignTareaCommand request, CancellationToken cancellationToken) {
            var assignTareaDTO = request.assignTareaDTO;

            var tarea = await tareaRepositorio.GetByIdAsync(assignTareaDTO.TareaId);
            if (tarea == null)
                throw new NotFoundException("La tarea no existe.");

            var usuarioExiste = await usuarioServicio.ValidateClientExistenceAsync(assignTareaDTO.UsuarioId);
            if (!usuarioExiste)
                throw new NotFoundException("El usuario no existe.");

            if (tarea.EstadoTarea == "Done")
                throw new ValidationException("No se puede asignar una tarea que ya está finalizada.");

            // Actualizar tarea. Si falla, lanza excepción desde el repositorio.
            await tareaRepositorio.UpdateClientAsync(assignTareaDTO.TareaId, assignTareaDTO.UsuarioId);

            if (tarea.UsuarioId.HasValue) {
                var message = new MessageDTO {
                    TareaId = tarea.Id,
                    UsuarioId = tarea.UsuarioId.Value,
                    CodigoTarea = tarea.CodigoTarea,
                    Titulo = tarea.Titulo
                };
                rabbitMQPublisher.PublishTaskAssigned(message);
            }

            return Unit.Value;
        }
    }
    
}
