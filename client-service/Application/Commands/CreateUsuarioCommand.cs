using client_service.Domain.Entities;
using client_service.Domain.ValueObjects;
using MediatR;

namespace client_service.Application.Commands
{
    public class CreateUsuarioCommand : IRequest<Usuario> {
        public string Identificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public Edad Edad { get; set; } = default!;
        public string? Cargo { get; set; }
        public Email Email { get; set; } = default!;
        public string Estado { get; set; } = "Activo";
    }
}
