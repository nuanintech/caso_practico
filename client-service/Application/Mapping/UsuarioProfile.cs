using AutoMapper;
using client_service.Application.Commands;
using client_service.Domain.Entities;
using client_service.Shared.DTOs;

namespace client_service.Application.Mapping {
    public class UsuarioProfile : Profile {
        public UsuarioProfile() {
            // DTO (cliente) → Command (CQRS)
            CreateMap<CreateUsuarioDTO, CreateUsuarioCommand>();
            CreateMap<UpdateUsuarioDTO, UpdateUsuarioCommand>();

            // Usuario Entity → DTO de respuesta
            CreateMap<Usuario, ResponseUsuarioDTO>();
            
        }
    }
}
