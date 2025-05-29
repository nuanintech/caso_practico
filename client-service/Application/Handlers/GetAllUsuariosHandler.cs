using client_service.Application.Queries;
using client_service.Domain.Entities;
using client_service.Domain.Interfaces;
using MediatR;

namespace client_service.Application.Handlers {
    public class GetAllUsuariosHandler : IRequestHandler<GetAllUsuariosQuery, IEnumerable<Usuario>> {
        private readonly IUsuarioRepositorio usuarioRepositorio;

        public GetAllUsuariosHandler(IUsuarioRepositorio repositorio) {
            this.usuarioRepositorio = repositorio;
        }

        public async Task<IEnumerable<Usuario>> Handle(GetAllUsuariosQuery request, CancellationToken cancellationToken) {
            return await usuarioRepositorio.GetAllAsync();
        }
    }
}