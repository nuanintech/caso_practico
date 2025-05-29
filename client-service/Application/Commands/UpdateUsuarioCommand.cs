using client_service.Domain.Entities;
using MediatR;

namespace client_service.Application.Commands {
    public class UpdateUsuarioCommand : IRequest<Usuario> {
        public Guid Id { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int? Edad { get; set; }
        public string? Cargo { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
