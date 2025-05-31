using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using task_service.Application.Commands;
using task_service.Application.Queries;
using task_service.Domain.Entities;
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
    }
}
