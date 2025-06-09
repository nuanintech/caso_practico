using MediatR;
using task_service.Domain.Entities;
using task_service.Shared.DTOs;

namespace task_service.Application.Commands
{
    public class UpdateTareaCommand : IRequest<Tarea> {
        #region Atributos
        public Guid Id { get; set; }
        public UpdateTareaDTO updateTareaDTO { get; }
        #endregion
        public UpdateTareaCommand(UpdateTareaDTO dto) {
            this.updateTareaDTO = dto;
        }
    }
}
