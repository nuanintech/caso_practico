﻿using AutoMapper;
using task_service.Application.Commands;
using task_service.Domain.Entities;
using task_service.Domain.Enum;
using task_service.Shared.DTOs;

namespace task_service.Application.Mapping {
    public class TareaProfile : Profile {
        public TareaProfile() {
            // DTO (tarea) → Command (CQRS)
            CreateMap<CreateTareaDTO, CreateTareaCommand>();
            //CreateMap<UpdateTareaDTO, UpdateUsuarioCommand>();

            // Task Entity → DTO de respuesta
            CreateMap<Tarea, ResponseTareaDTO>();

        }
    }
}
