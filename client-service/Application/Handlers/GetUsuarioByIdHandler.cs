using client_service.Application.Queries;
using client_service.Domain.Entities;
using client_service.Domain.Exceptions;
using client_service.Domain.Interfaces;
using MediatR;

namespace client_service.Application.Handlers {
    public class GetUsuarioByIdHandler : IRequestHandler<GetUsuarioByIdQuery, Usuario?> {
        private readonly IUsuarioRepositorio usuarioRepositorio;

        public GetUsuarioByIdHandler(IUsuarioRepositorio repositorio) {
            usuarioRepositorio = repositorio;
        }

        public async Task<Usuario?> Handle(GetUsuarioByIdQuery request, CancellationToken cancellationToken) {
            var usuario = await usuarioRepositorio.GetByIdAsync(request.Id);
            if (usuario is null)
                throw new NotFoundException($"No se encontró el usuario con Id : {request.Id}");

            return await usuarioRepositorio.GetByIdAsync(request.Id);
        }
    }
}
