using MediatR;
using task_service.Shared.DTOs;

namespace task_service.Application.Commands {
    public class UpdateTareaStateCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public UpdateTareaStateDTO updateStateDTO { get; }
        public UpdateTareaStateCommand(UpdateTareaStateDTO dto) {
            updateStateDTO = dto;
        }
    }
}
