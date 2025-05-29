using MediatR;
using task_service.Application.Commands;
using task_service.Domain.Entities;
using task_service.Domain.Exceptions;
using task_service.Domain.Interfaces;

namespace task_service.Application.Handlers
{
    public class CreateTareaHandler : IRequestHandler<CreateTareaCommand, Tarea> {
        private readonly ITareaRepositorio tareaRepositorio;

        public CreateTareaHandler(ITareaRepositorio tareaRepositorio) {
            this.tareaRepositorio = tareaRepositorio;
        }

        public async Task<Tarea> Handle(CreateTareaCommand request, CancellationToken cancellationToken) {
            var existente = await tareaRepositorio.GetByCodigoAsync(request.CodigoTarea);
            if (existente is not null)
                throw new ConflictException($"Ya existe una tarea con el código: {request.CodigoTarea}");

            int? tiempoDias = null;
            if (request.FechaInicio.HasValue && request.FechaFin.HasValue) {
                tiempoDias = (request.FechaFin.Value - request.FechaInicio.Value).Days;
            }

            var tarea = new Tarea {
                Id = Guid.NewGuid(),
                CodigoTarea = request.CodigoTarea,
                Titulo = request.Titulo,
                Descripcion = request.Descripcion,
                CriteriosAceptacion = request.CriteriosAceptacion,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                TiempoDias = tiempoDias,
                EstadoTarea = request.EstadoTarea.ToString(),  // Enum EstadoTarea
                Estado = request.Estado.ToString(),            // Enum Estado
                UsuarioId = request.UsuarioId
            };

            return await tareaRepositorio.CreateAsync(tarea);
        }
    }
}