using Npgsql;

using poc_dapper_postgres.Interfaces;
using poc_dapper_postgres.Mapping;

namespace poc_dapper_postgres.Implementations
{
    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        public IDatabaseConnection GetDatabaseConnection(string connectionString)
        {
            Mappers.Initialize();

            return new DatabaseConnection(new NpgsqlConnection(connectionString));
        }
    }
}
