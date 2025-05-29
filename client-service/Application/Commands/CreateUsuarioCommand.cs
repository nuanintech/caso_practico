using client_service.Domain.Entities;
using MediatR;

namespace client_service.Application.Commands
{
    public class CreateUsuarioCommand : IRequest<Usuario> {
        public string Identificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int? Edad { get; set; } = default!;
        public string? Cargo { get; set; }
        public string Email { get; set; } = default!;
        public string Estado { get; set; } = "Activo";
    }
}
