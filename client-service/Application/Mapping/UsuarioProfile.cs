using AutoMapper;
using client_service.Application.Commands;
using client_service.Domain.Entities;
using client_service.Domain.ValueObjects;
using client_service.Shared.DTOs;

namespace client_service.Application.Mapping {
    public class UsuarioProfile : Profile {
        public UsuarioProfile() {
            // DTO (cliente) → Command (CQRS)
            CreateMap<CreateUsuarioDTO, CreateUsuarioCommand>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.Edad));

            // Usuario Entity → DTO de respuesta
            CreateMap<Usuario, ResponseUsuarioDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.Edad));
        }
    }
}
