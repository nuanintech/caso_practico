using MediatR;
using task_service.Shared.DTOs;

namespace task_service.Application.Commands
{
    public class AssignTareaCommand : IRequest<Unit> {
        #region Atributos
        public AssignTareaDTO assignTareaDTO { get; }
        #endregion

        #region Constructor
        public AssignTareaCommand(AssignTareaDTO assignTareaDTO) {
            this.assignTareaDTO = assignTareaDTO;
        }
        #endregion
    }
}
