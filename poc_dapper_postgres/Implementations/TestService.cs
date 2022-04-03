using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using poc_dapper_postgres.Interfaces;
using poc_dapper_postgres.Models;
using poc_dapper_postgres.Queries.Interfaces;

namespace poc_dapper_postgres.Implementations
{
    public class TestService : ITestService
    {
        private readonly IContainerDatabaseService containerDatabaseService;
        private readonly IDatabaseConnectionFactory databaseConnectionFactory;

        private readonly ITestParentModelQueryBuilder testParentModelQueryBuilder;
        private readonly ITestModelQueryBuilder testModelQueryBuilder;

        public TestService(IContainerDatabaseService containerDatabaseService,
            IDatabaseConnectionFactory databaseConnectionFactory,
            ITestParentModelQueryBuilder testParentModelQueryBuilder,
            ITestModelQueryBuilder testModelQueryBuilder)
        {
            this.containerDatabaseService = containerDatabaseService;
            this.databaseConnectionFactory = databaseConnectionFactory;

            this.testParentModelQueryBuilder = testParentModelQueryBuilder;
            this.testModelQueryBuilder = testModelQueryBuilder;
        }

        public async Task RunAsync()
        {
            await containerDatabaseService.StartAsync();

            try
            {
                using (var connection = databaseConnectionFactory.GetDatabaseConnection(containerDatabaseService.GetConnectionString()))
                {
                    await connection.ExecuteRawQueryAsync("CREATE TABLE IF NOT EXISTS tblparenttest (id UUID PRIMARY KEY, name VARCHAR(255), creation_date_time timestamp)");

                    await connection.ExecuteRawQueryAsync("CREATE TABLE IF NOT EXISTS tbltest (id UUID PRIMARY KEY, parent_model_id UUID, num_date int, num_vlr decimal(16,8), CONSTRAINT fk_parent FOREIGN KEY (parent_model_id) REFERENCES tblparenttest(id))");

                    var parentModel = new TestParentModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Parent Model",
                        CreationDateTime = DateTime.Now
                    };

                    var model = new TestModel
                    {
                        Id = Guid.NewGuid(),
                        NumDate = 20220402,
                        NumValue = 123.456789m,
                        TestParentModelId = parentModel.Id
                    };

                    await InsertOne(connection, parentModel);

                    await InsertOne(connection, model);

                    await GetOne(connection, model);

                    await InsertMany(connection);

                    await UpdateOne(connection, model);

                    await GetOne(connection, model);

                    await DeleteOne(connection, model);

                    await GetOne(connection, model);

                    await BulkInsertTestModel(connection, parentModel.Id, 1000);

                    await BulkInsertTestModel(connection, parentModel.Id, 2000);

                    await BulkInsertTestModel(connection, parentModel.Id, 3000);

                    await BulkInsertTestModel(connection, parentModel.Id, 4000);

                    await BulkInsertTestModel(connection, parentModel.Id, 5000);

                    await BulkInsertTestModel(connection, parentModel.Id, 6000);

                    await BulkInsertTestModel(connection, parentModel.Id, 7000);

                    await BulkInsertTestModel(connection, parentModel.Id, 8000);

                    await BulkInsertTestModel(connection, parentModel.Id, 9000);

                    await BulkInsertTestModel(connection, parentModel.Id, 10000);

                    await GetCountTestModel(connection);

                    // ---

                    var testParentModel = new TestParentModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test Parent Model",
                        CreationDateTime = DateTime.Now
                    };

                    await InsertOne(connection, testParentModel);

                    await GetCountTestParentModel(connection);

                    await DeleteAllTestModel(connection);

                    await DeleteAllTestParentModel(connection);

                    await BulkInsertTestParentModel(connection, 100, 1000);

                    await GetCountTestParentModel(connection);

                    await GetCountTestModel(connection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oopss");
                Console.WriteLine(ex.Message);
            }

            await containerDatabaseService.StopAsync();
        }

        private async Task GetCountTestModel(IDatabaseConnection connection)
        {
            Console.WriteLine("Getting count...");

            var query = testModelQueryBuilder.BuildGetCount();

            var count = await connection.GetOneAsync<int>(query);

            Console.WriteLine("Count: {0}", count);
        }

        private async Task GetCountTestParentModel(IDatabaseConnection connection)
        {
            Console.WriteLine("Getting count...");

            var query = testParentModelQueryBuilder.BuildGetCount();

            var count = await connection.GetOneAsync<int>(query);

            Console.WriteLine("Count: {0}", count);
        }

        private async Task BulkInsertTestModel(IDatabaseConnection connection, Guid testParentModelId, int numOfModels)
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
                    NumValue = baseNumValue + 0.001m * i,
                    TestParentModelId = testParentModelId
                });
            }

            Console.WriteLine("Starting bulk insert...");

            var sw = Stopwatch.StartNew();

            await connection.BulkInsert(copyHelper, modelList);

            sw.Stop();

            Console.WriteLine("Finished bulk insert in {0}", sw.Elapsed);
        }

        private async Task BulkInsertTestParentModel(IDatabaseConnection connection, int numOfParentModels, int numOfModels)
        {
            const int baseNumDate = 20220101;
            const decimal baseNumValue = 0.001m;

            Console.WriteLine("Starting preparations for bulk insert of {0} records with {1} sub records...", numOfParentModels, numOfModels);

            var testParentModelCopyHelper = testParentModelQueryBuilder.BuildBulkCopyHelper();

            var testModelCopyHelper = testModelQueryBuilder.BuildBulkCopyHelper();

            var modelList = new List<TestParentModel>();

            for (int i = 0; i < numOfParentModels; i++)
            {
                var testParentModel = new TestParentModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Parent Model " + i,
                    CreationDateTime = DateTime.Now
                };

                testParentModel.TestModels = new List<TestModel>();

                for (int j = 0; j < numOfModels; j++)
                {
                    testParentModel.TestModels.Add(new TestModel
                    {
                        Id = Guid.NewGuid(),
                        NumDate = baseNumDate + j,
                        NumValue = baseNumValue + 0.001m * j,
                        TestParentModelId = testParentModel.Id
                    });
                }

                modelList.Add(testParentModel);
            }

            Console.WriteLine("Starting bulk insert...");

            var sw = Stopwatch.StartNew();

            await connection.BulkInsert(testParentModelCopyHelper, modelList);

            await connection.BulkInsert(testModelCopyHelper, modelList.SelectMany(x => x.TestModels));

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

        private async Task DeleteAllTestModel(IDatabaseConnection connection)
        {
            Console.WriteLine("Deleting all...");

            var query = testModelQueryBuilder.BuildDeleteAll();

            await connection.DeleteAsync(query);

            Console.WriteLine("Deleted");
        }

        private async Task DeleteAllTestParentModel(IDatabaseConnection connection)
        {
            Console.WriteLine("Deleting all...");

            var query = testParentModelQueryBuilder.BuildDeleteAll();

            await connection.DeleteAsync(query);

            Console.WriteLine("Deleted");
        }

        private async Task UpdateOne(IDatabaseConnection connection, TestModel model)
        {
            Console.WriteLine("Updating {0}...", model.Id);

            var query = testModelQueryBuilder.BuildUpdate(model.Id, new TestModel
            {
                NumDate = 0,
                NumValue = 0,
                TestParentModelId = model.TestParentModelId
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
            var parentModel = new TestParentModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Parent Model",
                CreationDateTime = DateTime.Now
            };

            var query = testParentModelQueryBuilder.BuildInsert(parentModel);

            await connection.InsertAsync(query);

            var modelList = new List<TestModel>
            {
                new TestModel
                {
                    Id = Guid.NewGuid(),
                    NumDate = 20220403,
                    NumValue = 123.456789m,
                    TestParentModelId = parentModel.Id
                },

                new TestModel
                {
                    Id = Guid.NewGuid(),
                    NumDate = 20220404,
                    NumValue = 123.456789m,
                    TestParentModelId = parentModel.Id
                },
            };

            Console.WriteLine("Inserting {0} records...", modelList.Count);

            query = testModelQueryBuilder.BuildInsertMany(modelList);

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

        private async Task InsertOne(IDatabaseConnection connection, TestParentModel model)
        {
            Console.WriteLine("Inserting one...");

            var query = testParentModelQueryBuilder.BuildInsert(model);

            await connection.InsertAsync(query);

            Console.WriteLine("Inserted");
        }
    }
}
