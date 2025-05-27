using System.Data;

namespace client_service.Domain.Interfaces {
    public interface IConnectionFactory {
        IDbConnection CreateConnection();
    }
}
