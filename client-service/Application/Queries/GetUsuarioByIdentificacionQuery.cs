using client_service.Domain.Entities;
using MediatR;

namespace client_service.Application.Queries {
    public class GetUsuarioByIdentificacionQuery : IRequest<Usuario?> {
        public string Identificacion { get; }

        public GetUsuarioByIdentificacionQuery(string identificacion) {
            Identificacion = identificacion;
        }
    }
}
