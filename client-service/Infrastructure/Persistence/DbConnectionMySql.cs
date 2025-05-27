using client_service.Domain.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;

namespace client_service.Infrastructure.Persistence
{
    public class DbConnectionMySql : IConnectionFactory {
        private readonly IConfiguration configuration;

        public DbConnectionMySql(IConfiguration configuration) {
            this.configuration = configuration;
        }

        public IDbConnection CreateConnection() {
            var connectionString = configuration.GetConnectionString("MysqlConnection");
            return new MySqlConnection(connectionString);
        }
    }
}
