using client_service.Domain.Entities;
using MediatR;

namespace client_service.Application.Queries {
    public class GetAllUsuariosQuery : IRequest<IEnumerable<Usuario>> {

    }
}
