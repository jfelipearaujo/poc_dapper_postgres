using System.Threading.Tasks;

namespace poc_dapper_postgres.Interfaces
{
    public interface IContainerDatabaseService
    {
        Task StartAsync();
        Task StopAsync();
        string GetConnectionString();
    }
}
