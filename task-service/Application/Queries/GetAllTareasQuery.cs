using MediatR;
using task_service.Domain.Entities;

namespace task_service.Application.Queries {
    public class GetAllTareasQuery : IRequest<IEnumerable<Tarea>> {

    }
}

