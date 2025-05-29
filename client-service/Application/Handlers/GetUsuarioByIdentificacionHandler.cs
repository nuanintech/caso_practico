using client_service.Application.Queries;
using client_service.Domain.Entities;
using client_service.Domain.Exceptions;
using client_service.Domain.Interfaces;
using MediatR;

namespace client_service.Application.Handlers {
    public class GetUsuarioByIdentificacionHandler : IRequestHandler<GetUsuarioByIdentificacionQuery, Usuario?> {
        private readonly IUsuarioRepositorio usuarioRepositorio;

        public GetUsuarioByIdentificacionHandler(IUsuarioRepositorio repositorio) {
            usuarioRepositorio = repositorio;
        }

        public async Task<Usuario?> Handle(GetUsuarioByIdentificacionQuery request, CancellationToken cancellationToken) {
            var usuario = await usuarioRepositorio.GetByIdentificationAsync(request.Identificacion);
            if (usuario is null)
                throw new NotFoundException($"No se encontró el usuario con Identificación : {request.Identificacion}");

            return usuario;
        }
    }
}
