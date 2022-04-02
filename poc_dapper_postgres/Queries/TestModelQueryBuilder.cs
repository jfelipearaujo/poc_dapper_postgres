using System;
using System.Collections.Generic;

using poc_dapper_postgres.Models;

using PostgreSQLCopyHelper;

using SqlKata;

namespace poc_dapper_postgres.Queries
{
    public class TestModelQueryBuilder
    {
        private const string TABLE_NAME = "tbltest";

        public Query BuildInsert(TestModel entity)
        {
            var columns = new[]
            {
                "id",
                "num_date",
                "num_vlr"
            };

            var data = new List<object[]>
            {
                new object[]
                {
                    entity.Id,
                    entity.NumDate,
                    entity.NumValue
                }
            };

            return new Query(TABLE_NAME)
                .AsInsert(columns, data);
        }

        public Query BuildInsertMany(List<TestModel> entities)
        {
            var columns = new[]
            {
                "id",
                "num_date",
                "num_vlr"
            };

            var data = new List<object[]>();

            foreach (var entity in entities)
            {
                data.Add(new object[] {
                    entity.Id,
                    entity.NumDate,
                    entity.NumValue
                });
            }

            return new Query(TABLE_NAME)
                .AsInsert(columns, data);
        }

        public PostgreSQLCopyHelper<TestModel> BuildBulkCopyHelper()
        {
            return new PostgreSQLCopyHelper<TestModel>(TABLE_NAME)
                .MapUUID("id", x => x.Id)
                .MapInteger("num_date", x => x.NumDate)
                .MapNumeric("num_vlr", x => x.NumValue);
        }

        public Query BuildGetCount()
        {
            return new Query(TABLE_NAME)
                .SelectRaw("count(*) as count");
        }

        public Query BuildGetById(Guid id)
        {
            return new Query(TABLE_NAME)
                .Select("id", "num_date", "num_vlr")
                .Where("id", "=", id);
        }

        public Query BuildGetAll()
        {
            return new Query(TABLE_NAME)
                .Select("id", "num_date", "num_vlr");
        }

        public Query BuildUpdate(Guid id, TestModel newEntity)
        {
            var columns = new[]
            {
                "num_date",
                "num_vlr"
            };

            var data = new object[]
            {
                newEntity.NumDate,
                newEntity.NumValue
            };

            return new Query(TABLE_NAME)
                .Where("id", "=", id)
                .AsUpdate(columns, data);
        }

        public Query BuildDelete(Guid id)
        {
            return new Query(TABLE_NAME)
                .Where("id", "=", id)
                .AsDelete();
        }
    }
}
