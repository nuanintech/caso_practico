using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using task_service.Application.Commands;
using task_service.Application.Queries;
using task_service.Domain.Entities;
using task_service.Domain.Interfaces;
using task_service.Shared.DTOs;

namespace task_service.API.Controllers {
    [ApiController]
    [Route("api/v1/task")]
    public class TareaController : ControllerBase {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ILogger<TareaController> logger;
        public TareaController(IMediator mediator, IMapper mapper, ILogger<TareaController> logger) {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTareaDTO createTareaDTO) {
            // 1. Mapeamos el DTO al Command
            var command = new CreateTareaCommand(createTareaDTO);

            // 2. Ejecutamos el flujo con MediatR (con validación automática)
            Tarea tarea = await mediator.Send(command);

            // 3. Mapeamos a la respuesta y devolvemos 201 Created
            var response = mapper.Map<ResponseTareaDTO>(tarea);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTareaDTO updateTareaDTO) {
            // 1. Mapeamos el DTO al Command
            var command = new UpdateTareaCommand(updateTareaDTO);
            command.Id = id;

            // 2. Ejecutamos el flujo con MediatR (con validación automática)
            Tarea tarea = await mediator.Send(command);

            // 3. Mapeamos a la respuesta y devolvemos 200 OK
            var response = mapper.Map<ResponseTareaDTO>(tarea);
            return Ok(response);
        }
        [HttpPatch("{id}/state")]
        public async Task<IActionResult> UpdateState(Guid id, [FromBody] UpdateTareaStateDTO updateStateDTO) {
            // 1. Mapeamos el DTO al Command
            var command = new UpdateTareaStateCommand(updateStateDTO);
            command.Id = id;

            // 2. Ejecutamos el flujo con MediatR (con validación automática)
            await mediator.Send(command);

            // 3. Devolvemos 204 NoContent
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            // 1. Ejecutamos el flujo con MediatR (con validación automática)
            await mediator.Send(new DeleteTareaCommand(id));

            // 2. Devolvemos 204 NoContent
            return NoContent();
        }
        [HttpPatch("assignclient")]
        public async Task<IActionResult> Assign([FromBody] AssignTareaDTO assignTareaDTO) {
            // 1. Mapeamos el DTO al Command
            var command = new AssignTareaCommand(assignTareaDTO);

            // 2. Ejecutamos el flujo con MediatR (con validación automática)
            await mediator.Send(command);

            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            // 1. Ejecutamos el flujo con MediatR (con validación automática)
            IEnumerable<Tarea> tareas = await mediator.Send(new GetAllTareasQuery());

            // 2. Mapeamos a la respuesta y devolvemos 200 OK
            var response = mapper.Map<IEnumerable<ResponseTareaDTO>>(tareas);

            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) {
            // 1. Ejecutamos el flujo con MediatR (con validación automática)
            var tarea = await mediator.Send(new GetTareaByIdQuery(id));
            
            // 2. Mapeamos a la respuesta y devolvemos 200 OK
            var response = mapper.Map<ResponseTareaDTO>(tarea);
            
            return Ok(response);
        }
        [HttpGet("user/{usuarioId}")]
        public async Task<IActionResult> GetByUsuarioId(Guid usuarioId) {
            // 1. Ejecutamos el flujo con MediatR (con validación automática)
            var tareas = await mediator.Send(new GetTareasByUsuarioIdQuery(usuarioId));

            // 2. Mapeamos a la respuesta y devolvemos 200 OK
            var response = mapper.Map<IEnumerable<ResponseTareaDTO>>(tareas);
            return Ok(response);
        }
    }
}
