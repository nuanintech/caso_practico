using MediatR;
using task_service.Domain.Entities;
using task_service.Domain.Enum;
using task_service.Shared.DTOs;

namespace task_service.Application.Commands {
    public class CreateTareaCommand : IRequest<Tarea> {
        #region Atributos
        public CreateTareaDTO createTareaDTO{ get; }
        #endregion
        public CreateTareaCommand(CreateTareaDTO dto) {
            this.createTareaDTO = dto;
        }
    }
}
