using MediatR;

namespace task_service.Application.Commands
{
    public class DeleteTareaCommand : IRequest<Unit> {
        public Guid Id { get; }

        public DeleteTareaCommand(Guid id) {
            Id = id;
        }
    }
}
