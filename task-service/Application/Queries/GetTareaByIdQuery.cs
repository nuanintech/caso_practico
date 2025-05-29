using MediatR;
using task_service.Domain.Entities;

namespace task_service.Application.Queries
{
    public class GetTareaByIdQuery : IRequest<Tarea?> {
        public Guid Id { get; }

        public GetTareaByIdQuery(Guid id) {
            Id = id;
        }
    }
}
