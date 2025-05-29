using client_service.Domain.Entities;
using MediatR;

namespace client_service.Application.Queries
{
    public class GetUsuarioByEmailQuery : IRequest<Usuario?> {
        public string Email { get; }

        public GetUsuarioByEmailQuery(string email) {
            Email = email;
        }
    }
}
