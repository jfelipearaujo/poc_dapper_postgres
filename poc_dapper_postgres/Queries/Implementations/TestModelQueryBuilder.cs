using System;
using System.Collections.Generic;
using System.Linq;

using poc_dapper_postgres.Models;
using poc_dapper_postgres.Queries.Interfaces;

using PostgreSQLCopyHelper;

using SqlKata;

namespace poc_dapper_postgres.Queries.Implementations
{
    public class TestModelQueryBuilder : ITestModelQueryBuilder
    {
        private const string TABLE_NAME = "tbltest";

        private readonly string[] ALL_COLUMNS = new[]
        {
            "id",
            "num_date",
            "num_vlr",
            "parent_model_id"
        };

        public PostgreSQLCopyHelper<TestModel> BuildBulkCopyHelper()
        {
            return new PostgreSQLCopyHelper<TestModel>(TABLE_NAME)
                .MapUUID("id", x => x.Id)
                .MapInteger("num_date", x => x.NumDate)
                .MapNumeric("num_vlr", x => x.NumValue)
                .MapUUID("parent_model_id", x => x.TestParentModelId);
        }

        public Query BuildDelete(Guid id)
        {
            return new Query(TABLE_NAME)
                .Where("id", "=", id)
                .AsDelete();
        }

        public Query BuildDeleteAll()
        {
            return new Query(TABLE_NAME)
                .AsDelete();
        }

        public Query BuildGetAll()
        {
            return new Query(TABLE_NAME)
                .Select(ALL_COLUMNS);
        }

        public Query BuildGetById(Guid id)
        {
            return new Query(TABLE_NAME)
                .Select(ALL_COLUMNS)
                .Where("id", "=", id);
        }

        public Query BuildGetCount()
        {
            return new Query(TABLE_NAME)
                .SelectRaw("count(*) as count");
        }

        public Query BuildInsert(TestModel entity)
        {
            var data = new List<object[]>
            {
                new object[]
                {
                    entity.Id,
                    entity.NumDate,
                    entity.NumValue,
                    entity.TestParentModelId
                }
            };

            return new Query(TABLE_NAME)
                .AsInsert(ALL_COLUMNS, data);
        }

        public Query BuildInsertMany(List<TestModel> entities)
        {
            var data = new List<object[]>();

            foreach (var entity in entities)
            {
                data.Add(new object[] {
                    entity.Id,
                    entity.NumDate,
                    entity.NumValue,
                    entity.TestParentModelId
                });
            }

            return new Query(TABLE_NAME)
                .AsInsert(ALL_COLUMNS, data);
        }

        public Query BuildUpdate(Guid id, TestModel newEntity)
        {
            var columns = ALL_COLUMNS.Where(x => !x.Equals("id"));

            var data = new object[]
            {
                newEntity.NumDate,
                newEntity.NumValue,
                newEntity.TestParentModelId
            };

            return new Query(TABLE_NAME)
                .Where("id", "=", id)
                .AsUpdate(columns, data);
        }

    }
}
