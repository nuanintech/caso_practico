using AutoMapper;
using client_service.Application.Commands;
using client_service.Domain.Entities;
using client_service.Domain.Exceptions;
using client_service.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace client_service.API.Controllers {
    [ApiController]
    [Route("api/v1/client")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ILogger<UsuarioController> logger;
        public UsuarioController(IMediator mediator, IMapper mapper, ILogger<UsuarioController> logger) {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUsuarioDTO createUsuarioDTO) {
            // 1. Mapeamos el DTO al Command
            var command = mapper.Map<CreateUsuarioCommand>(createUsuarioDTO);

            // 2. Ejecutamos el flujo con MediatR (con validación automática)
            Usuario usuario = await mediator.Send(command);

            // 3. Mapeamos a la respuesta y devolvemos 201 Created
            var response = mapper.Map<ResponseUsuarioDTO>(usuario);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id) {
            return Ok(); // Implementación futura
        }
    }
}
