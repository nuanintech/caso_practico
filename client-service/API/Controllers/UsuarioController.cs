using AutoMapper;
using client_service.Application.Commands;
using client_service.Application.Queries;
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUsuarioDTO updateUsuarioDTO) {
            // 1. Mapeamos el DTO al Command
            var command = mapper.Map<UpdateUsuarioCommand>(updateUsuarioDTO);

            // 2. Asignamos el ID de la URL a el Command
            command.Id = id;

            // 3. Ejecutamos el flujo con MediatR (con validación automática)
            var usuario = await mediator.Send(command);

            // 3. Mapeamos a la respuesta y devolvemos 200 OK
            var response = mapper.Map<ResponseUsuarioDTO>(usuario);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            // 1. Ejecutamos el flujo con MediatR (con validación automática)
            await mediator.Send(new DeleteUsuarioCommand(id));

            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            // 1. Ejecutamos el flujo con MediatR (con validación automática)
            IEnumerable<Usuario> usuarios = await mediator.Send(new GetAllUsuariosQuery());
            // 2. Mapeamos a la respuesta y devolvemos 200 OK
            var response = mapper.Map<IEnumerable<ResponseUsuarioDTO>>(usuarios);
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) {
            // 1. Ejecutamos el flujo con MediatR (con validación automática)
            var usuario = await mediator.Send(new GetUsuarioByIdQuery(id));
            // 2. Mapeamos a la respuesta y devolvemos 200 OK
            var response = mapper.Map<ResponseUsuarioDTO>(usuario);
            return Ok(response);
        }
        [HttpGet("identificacion/{identificacion}")]
        public async Task<IActionResult> GetByIdentificacion(string identificacion) {
            // 1. Ejecutamos el flujo con MediatR (con validación automática)
            var usuario = await mediator.Send(new GetUsuarioByIdentificacionQuery(identificacion));
            // 2. Mapeamos a la respuesta y devolvemos 200 OK
            var response = mapper.Map<ResponseUsuarioDTO>(usuario);
            return Ok(response);
        }
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email) {
            // 1. Ejecutamos el flujo con MediatR (con validación automática)
            var usuario = await mediator.Send(new GetUsuarioByEmailQuery(email));
            // 2. Mapeamos a la respuesta y devolvemos 200 OK
            var response = mapper.Map<ResponseUsuarioDTO>(usuario);
            return Ok(response);
        }
    }
}
