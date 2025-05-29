using client_service.Domain.Entities;
using MediatR;

namespace client_service.Application.Queries {
    public class GetUsuarioByIdQuery : IRequest<Usuario?> {
        public Guid Id { get; }

        public GetUsuarioByIdQuery(Guid id) {
            Id = id;
        }
    }
}
