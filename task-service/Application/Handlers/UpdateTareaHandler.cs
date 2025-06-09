using MediatR;
using task_service.Application.Commands;
using task_service.Domain.Entities;
using task_service.Domain.Exceptions;
using task_service.Domain.Interfaces;
using task_service.Shared.DTOs;

namespace task_service.Application.Handlers
{
    public class UpdateTareaHandler : IRequestHandler<UpdateTareaCommand, Tarea>
    {
        private readonly ITareaRepositorio tareaRepositorio;

        public UpdateTareaHandler(ITareaRepositorio tareaRepositorio) {
            this.tareaRepositorio = tareaRepositorio;
        }

        public async Task<Tarea> Handle(UpdateTareaCommand request, CancellationToken cancellationToken) {
            var updateTareaDTO = request.updateTareaDTO;

            var tarea = await tareaRepositorio.GetByIdAsync(request.Id);
            if (tarea is null)
                throw new NotFoundException($"No se encontró la tarea con Id : {request.Id}");

            if (!tarea.CodigoTarea.Equals(updateTareaDTO.CodigoTarea)) {
                var existente = await tareaRepositorio.GetByCodigoAsync(updateTareaDTO.CodigoTarea);
                if (existente is not null)
                    throw new ConflictException($"Ya existe una tarea con el código: {updateTareaDTO.CodigoTarea}");
            }

            int? tiempoDias = null;
            if (updateTareaDTO.FechaInicio.HasValue && updateTareaDTO.FechaFin.HasValue) {
                tiempoDias = (updateTareaDTO.FechaFin.Value - updateTareaDTO.FechaInicio.Value).Days;
            }

            tarea.Id = request.Id;
            tarea.CodigoTarea = updateTareaDTO.CodigoTarea;
            tarea.Titulo = updateTareaDTO.Titulo;
            tarea.Descripcion = updateTareaDTO.Descripcion;
            tarea.CriteriosAceptacion = updateTareaDTO.CriteriosAceptacion;
            tarea.FechaInicio = updateTareaDTO.FechaInicio;
            tarea.FechaFin = updateTareaDTO.FechaFin;
            tarea.TiempoDias = tiempoDias;

            await tareaRepositorio.UpdateAsync(tarea);

            return tarea;
        }
    }
}