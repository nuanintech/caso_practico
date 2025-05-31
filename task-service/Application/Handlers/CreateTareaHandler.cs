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
            var createTareaDTO = request.createTareaDTO;

            var existente = await tareaRepositorio.GetByCodigoAsync(createTareaDTO.CodigoTarea);
            if (existente is not null)
                throw new ConflictException($"Ya existe una tarea con el código: {createTareaDTO.CodigoTarea}");

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

            return await tareaRepositorio.CreateAsync(tarea);
        }
    }
}