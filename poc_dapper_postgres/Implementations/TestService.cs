using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using poc_dapper_postgres.Interfaces;
using poc_dapper_postgres.Models;
using poc_dapper_postgres.Queries;

namespace poc_dapper_postgres.Implementations
{
    public class TestService : ITestService
    {
        private readonly IContainerDatabaseService containerDatabaseService;
        private readonly IDatabaseConnectionFactory databaseConnectionFactory;

        private readonly TestModelQueryBuilder testModelQueryBuilder = new();

        public TestService(IContainerDatabaseService containerDatabaseService,
            IDatabaseConnectionFactory databaseConnectionFactory)
        {
            this.containerDatabaseService = containerDatabaseService;
            this.databaseConnectionFactory = databaseConnectionFactory;
        }

        public async Task RunAsync()
        {
            await containerDatabaseService.StartAsync();

            try
            {
                using (var connection = databaseConnectionFactory.GetDatabaseConnection(containerDatabaseService.GetConnectionString()))
                {
                    await connection.ExecuteRawQueryAsync("CREATE TABLE IF NOT EXISTS tbltest (id UUID PRIMARY KEY, num_date int, num_vlr decimal(16,8))");

                    var model = new TestModel
                    {
                        Id = Guid.NewGuid(),
                        NumDate = 20220402,
                        NumValue = 123.456789m
                    };

                    await InsertOne(connection, model);

                    await GetOne(connection, model);

                    await InsertMany(connection);

                    await UpdateOne(connection, model);

                    await GetOne(connection, model);

                    await DeleteOne(connection, model);

                    await GetOne(connection, model);

                    await BulkInsert(connection, 1000);

                    await BulkInsert(connection, 2000);

                    await BulkInsert(connection, 3000);

                    await BulkInsert(connection, 4000);

                    await BulkInsert(connection, 5000);

                    await BulkInsert(connection, 6000);

                    await BulkInsert(connection, 7000);

                    await BulkInsert(connection, 8000);

                    await BulkInsert(connection, 9000);

                    await BulkInsert(connection, 10000);

                    await GetCount(connection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oopss");
                Console.WriteLine(ex.Message);
            }

            await containerDatabaseService.StopAsync();
        }

        private async Task GetCount(IDatabaseConnection connection)
        {
            Console.WriteLine("Getting count...");

            var query = testModelQueryBuilder.BuildGetCount();

            var count = await connection.GetOneAsync<int>(query);

            Console.WriteLine("Count: {0}", count);
        }

        private async Task BulkInsert(IDatabaseConnection connection, int numOfModels)
        {
            const int baseNumDate = 20220101;
            const decimal baseNumValue = 0.001m;

            Console.WriteLine("Starting preparations for bulk insert of {0} records...", numOfModels);

            var copyHelper = testModelQueryBuilder.BuildBulkCopyHelper();

            var modelList = new List<TestModel>();

            for (int i = 0; i < numOfModels; i++)
            {
                modelList.Add(new TestModel
                {
                    Id = Guid.NewGuid(),
                    NumDate = baseNumDate + i,
                    NumValue = baseNumValue + 0.001m * i
                });
            }

            Console.WriteLine("Starting bulk insert...");

            var sw = Stopwatch.StartNew();

            await connection.BulkInsert(copyHelper, modelList);

            sw.Stop();

            Console.WriteLine("Finished bulk insert in {0}", sw.Elapsed);
        }

        private async Task DeleteOne(IDatabaseConnection connection, TestModel model)
        {
            Console.WriteLine("Deleting {0}...", model.Id);

            var query = testModelQueryBuilder.BuildDelete(model.Id);

            await connection.DeleteAsync(query);

            Console.WriteLine("Deleted");
        }

        private async Task UpdateOne(IDatabaseConnection connection, TestModel model)
        {
            Console.WriteLine("Updating {0}...", model.Id);

            var query = testModelQueryBuilder.BuildUpdate(model.Id, new TestModel
            {
                NumDate = 0,
                NumValue = 0
            });

            await connection.UpdateAsync(query);

            Console.WriteLine("Updated");
        }

        private async Task GetAll(IDatabaseConnection connection)
        {
            Console.WriteLine("Getting all...");

            var query = testModelQueryBuilder.BuildGetAll();

            var resultList = await connection.GetAsync<TestModel>(query);

            if (resultList.Any())
            {
                Console.WriteLine("Found {0} records", resultList.Count());

                foreach (var item in resultList)
                {
                    Console.WriteLine($"{item.Id} - {item.NumDate} - {item.NumValue}");
                }
            }
            else
            {
                Console.WriteLine("No records found");
            }
        }

        private async Task InsertMany(IDatabaseConnection connection)
        {
            var modelList = new List<TestModel>
            {
                new TestModel
                {
                    Id = Guid.NewGuid(),
                    NumDate = 20220403,
                    NumValue = 123.456789m
                },

                new TestModel
                {
                    Id = Guid.NewGuid(),
                    NumDate = 20220404,
                    NumValue = 123.456789m
                },
            };

            Console.WriteLine("Inserting {0} records...", modelList.Count);

            var query = testModelQueryBuilder.BuildInsertMany(modelList);

            await connection.InsertAsync(query);

            Console.WriteLine("Inserted");

            await GetAll(connection);

            await DeleteOne(connection, modelList[0]);

            await DeleteOne(connection, modelList[1]);
        }

        private async Task GetOne(IDatabaseConnection connection, TestModel model)
        {
            Console.WriteLine("Getting by id {0}...", model.Id);

            var query = testModelQueryBuilder.BuildGetById(model.Id);

            var result = await connection.GetOneAsync<TestModel>(query);

            if (result is not null)
            {
                Console.WriteLine($"{result.Id} - {result.NumDate} - {result.NumValue}");
            }
            else
            {
                Console.WriteLine("No record found");
            }
        }

        private async Task InsertOne(IDatabaseConnection connection, TestModel model)
        {
            Console.WriteLine("Inserting one...");

            var query = testModelQueryBuilder.BuildInsert(model);

            await connection.InsertAsync(query);

            Console.WriteLine("Inserted");
        }
    }
}
