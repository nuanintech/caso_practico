using client_service.Application.Commands;
using client_service.Domain.Exceptions;
using client_service.Domain.Interfaces;
using MediatR;

namespace client_service.Application.Handlers
{
    public class DeleteUsuarioHandler : IRequestHandler<DeleteUsuarioCommand, Unit> {
        private readonly IUsuarioRepositorio usuarioRepositorio;

        public DeleteUsuarioHandler(IUsuarioRepositorio repositorio) {
            this.usuarioRepositorio = repositorio;
        }

        public async Task<Unit> Handle(DeleteUsuarioCommand request, CancellationToken cancellationToken) {
            var usuario = await usuarioRepositorio.GetByIdAsync(request.Id);
            if (usuario is null)
                throw new NotFoundException($"No se encontró el usuario con Id : {request.Id}");

            await usuarioRepositorio.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
