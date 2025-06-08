using MediatR;
using task_service.Application.Commands;
using task_service.Domain.Entities;
using task_service.Domain.Exceptions;
using task_service.Domain.Interfaces;
using task_service.Infrastructure.Services;
using task_service.Shared.DTOs;

namespace task_service.Application.Handlers
{
    public class CreateTareaHandler : IRequestHandler<CreateTareaCommand, Tarea> {
        private readonly ITareaRepositorio tareaRepositorio;
        private readonly IUsuarioService usuarioServicio;
        private readonly IRabbitMQPublisher rabbitMQPublisher;

        public CreateTareaHandler(ITareaRepositorio tareaRepositorio, IUsuarioService usuarioServicio, IRabbitMQPublisher rabbitMQPublisher) {
            this.tareaRepositorio = tareaRepositorio;
            this.rabbitMQPublisher = rabbitMQPublisher;
            this.usuarioServicio = usuarioServicio;
        }

        public async Task<Tarea> Handle(CreateTareaCommand request, CancellationToken cancellationToken) {
            var createTareaDTO = request.createTareaDTO;

            var existente = await tareaRepositorio.GetByCodigoAsync(createTareaDTO.CodigoTarea);
            if (existente is not null)
                throw new ConflictException($"Ya existe una tarea con el código: {createTareaDTO.CodigoTarea}");

            if (createTareaDTO.UsuarioId.HasValue) { 
                var usuarioExiste = await usuarioServicio.ValidateClientExistenceAsync(createTareaDTO.UsuarioId.Value);
                if (!usuarioExiste)
                    throw new NotFoundException("El usuario no existe.");

                if (createTareaDTO.EstadoTarea == "Done")
                    throw new ValidationException("No se puede asignar una tarea que ya está finalizada.");
            }
            

            int? tiempoDias = null;
            if (createTareaDTO.FechaInicio.HasValue && createTareaDTO.FechaFin.HasValue) {
                tiempoDias = (createTareaDTO.FechaFin.Value - createTareaDTO.FechaInicio.Value).Days;
            }

            var tarea = new Tarea {
                Id = Guid.NewGuid(),
                CodigoTarea = createTareaDTO.CodigoTarea,
                Titulo = createTareaDTO.Titulo,
                Descripcion = createTareaDTO.Descripcion,
                CriteriosAceptacion = createTareaDTO.CriteriosAceptacion,
                FechaInicio = createTareaDTO.FechaInicio,
                FechaFin = createTareaDTO.FechaFin,
                TiempoDias = tiempoDias,
                EstadoTarea = createTareaDTO.EstadoTarea.ToString(),  // Enum EstadoTarea
                Estado = "Activo",            // Enum Estado
                UsuarioId = createTareaDTO.UsuarioId
            };

            tarea = await tareaRepositorio.CreateAsync(tarea);
            if (tarea.UsuarioId.HasValue) {
                var message = new MessageDTO {
                    TareaId = tarea.Id,
                    UsuarioId = tarea.UsuarioId.Value,
                    CodigoTarea = tarea.CodigoTarea,
                    Titulo = tarea.Titulo
                };
                rabbitMQPublisher.PublishTaskAssigned(message);
            }

            return tarea;
        }
    }
}