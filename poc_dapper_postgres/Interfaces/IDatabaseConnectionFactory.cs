namespace poc_dapper_postgres.Interfaces
{
    public interface IDatabaseConnectionFactory
    {
        IDatabaseConnection GetDatabaseConnection(string connectionString);
    }
}
