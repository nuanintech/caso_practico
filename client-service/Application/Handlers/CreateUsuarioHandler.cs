using client_service.Application.Commands;
using client_service.Domain.Entities;
using client_service.Domain.Exceptions;
using client_service.Domain.Interfaces;
using MediatR;

namespace client_service.Application.Handlers
{
    public class CreateUsuarioHandler : IRequestHandler<CreateUsuarioCommand, Usuario> {
        private readonly IUsuarioRepositorio usuarioRepositorio;
        public CreateUsuarioHandler(IUsuarioRepositorio repositorio) {
            this.usuarioRepositorio = repositorio;
        }
        public async Task<Usuario> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken) {
            var existente = await usuarioRepositorio.GetByIdentificationAsync(request.Identificacion);
            if (existente is not null)
                throw new ConflictException($"Ya existe un usuario con la identificación: {request.Identificacion}");

            var usuario = new Usuario {
                Id = Guid.NewGuid(),
                Identificacion = request.Identificacion,
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                Edad = request.Edad,
                Email = request.Email,
                Cargo = request.Cargo,
                Estado = request.Estado
            };

            return await usuarioRepositorio.CreateAsync(usuario);
        }
    }
}

