using client_service.Application.Commands;
using client_service.Domain.Entities;
using client_service.Domain.Exceptions;
using client_service.Domain.Interfaces;
using MediatR;

namespace client_service.Application.Handlers
{
    public class UpdateUsuarioHandler : IRequestHandler<UpdateUsuarioCommand, Usuario> {
        private readonly IUsuarioRepositorio usuarioRepositorio;

        public UpdateUsuarioHandler(IUsuarioRepositorio repositorio) {
            this.usuarioRepositorio = repositorio;
        }

        public async Task<Usuario> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken) {
            var usuario = await usuarioRepositorio.GetByIdAsync(request.Id);
            if (usuario is null)
                throw new NotFoundException($"No se encontró el usuario con Id : {request.Id}");

            usuario.Identificacion = request.Identificacion;
            usuario.Nombres = request.Nombres;
            usuario.Apellidos = request.Apellidos;
            usuario.Edad = request.Edad;
            usuario.Cargo = request.Cargo;
            usuario.Email = request.Email;

            await usuarioRepositorio.UpdateAsync(usuario);

            return usuario;
        }
    }
}
