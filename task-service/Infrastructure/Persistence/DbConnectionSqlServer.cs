
using Microsoft.Data.SqlClient;
using System.Data;
using task_service.Domain.Interfaces;

namespace task_service.Infrastructure.Persistence {
    public class DbConnectionSqlServer : IConnectionFactory {
        private readonly IConfiguration configuration;

        public DbConnectionSqlServer(IConfiguration configuration) {
            this.configuration = configuration;
        }

        public IDbConnection CreateConnection() {
            var connectionString = configuration.GetConnectionString("SqlServerConnection");
            return new SqlConnection(connectionString);
        }
    }
}
