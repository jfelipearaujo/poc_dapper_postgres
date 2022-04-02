using System;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Databases;

using poc_dapper_postgres.Interfaces;

namespace poc_dapper_postgres.Implementations
{
    public class ContainerDatabaseService : IContainerDatabaseService
    {
        private readonly PostgreSqlTestcontainer container;

        public ContainerDatabaseService()
        {
            container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithDatabase(new PostgreSqlTestcontainerConfiguration
                {
                    Database = "db",
                    Username = "postgres",
                    Password = "postgres",
                })
               .Build();
        }

        public async Task StartAsync()
        {
            Console.WriteLine("Starting container...");
            await container.StartAsync();
            Console.WriteLine("Container started.");
        }

        public async Task StopAsync()
        {
            Console.WriteLine("Stopping container...");
            await container.StopAsync();
            Console.WriteLine("Container stopped.");

            Console.WriteLine("Cleanning up...");
            await container.CleanUpAsync();
            Console.WriteLine("Cleaned up.");
        }

        public string GetConnectionString()
        {
            return container.ConnectionString;
        }
    }
}
