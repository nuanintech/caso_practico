using MediatR;

namespace client_service.Application.Commands
{
    public class DeleteUsuarioCommand : IRequest<Unit> {
        public Guid Id { get; }

        public DeleteUsuarioCommand(Guid id) {
            Id = id;
        }
    }
}
