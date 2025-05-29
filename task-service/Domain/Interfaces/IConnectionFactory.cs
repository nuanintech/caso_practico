using System.Data;

namespace task_service.Domain.Interfaces
{
    public interface IConnectionFactory {
        IDbConnection CreateConnection();
    }
}

