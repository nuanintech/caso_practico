using task_service.Shared.DTOs;

namespace task_service.Domain.Interfaces
{
    public interface IRabbitMQPublisher {
        void PublishTaskAssigned(MessageDTO messageDTO);
    }
}
