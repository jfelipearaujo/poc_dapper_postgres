using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using poc_dapper_postgres.Implementations;
using poc_dapper_postgres.Interfaces;

namespace poc_dapper_postgres
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddTransient<ITestService, TestService>();
            services.AddTransient<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
            services.AddTransient<IContainerDatabaseService, ContainerDatabaseService>();

            var provider = services.BuildServiceProvider();

            var testService = provider.GetRequiredService<ITestService>();

            await testService.RunAsync();
        }
    }
}
