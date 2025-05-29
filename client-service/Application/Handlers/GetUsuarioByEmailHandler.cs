using client_service.Application.Queries;
using client_service.Domain.Entities;
using client_service.Domain.Exceptions;
using client_service.Domain.Interfaces;
using MediatR;

namespace client_service.Application.Handlers {
    public class GetUsuarioByEmailHandler : IRequestHandler<GetUsuarioByEmailQuery, Usuario?> {
        private readonly IUsuarioRepositorio usuarioRepositorio;

        public GetUsuarioByEmailHandler(IUsuarioRepositorio repositorio) {
            usuarioRepositorio = repositorio;
        }

        public async Task<Usuario?> Handle(GetUsuarioByEmailQuery request, CancellationToken cancellationToken) {
            var usuario = await usuarioRepositorio.GetByEmailAsync(request.Email);
            if (usuario is null)
                throw new NotFoundException($"No se encontró el usuario con Email : {request.Email}");

            return usuario;
        }
    }
}
