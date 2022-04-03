using System;
using System.Collections.Generic;

using poc_dapper_postgres.Models;
using poc_dapper_postgres.Queries.Interfaces;

using PostgreSQLCopyHelper;

using SqlKata;

namespace poc_dapper_postgres.Queries.Implementations
{
    public class TestParentModelQueryBuilder : ITestParentModelQueryBuilder
    {
        private const string TABLE_NAME = "tblparenttest";

        private readonly string[] ALL_COLUMNS = new[]
        {
            "id",
            "name",
            "creation_date_time"
        };

        public PostgreSQLCopyHelper<TestParentModel> BuildBulkCopyHelper()
        {
            return new PostgreSQLCopyHelper<TestParentModel>(TABLE_NAME)
                .MapUUID("id", x => x.Id)
                .MapVarchar("name", x => x.Name)
                .MapTimeStamp("creation_date_time", x => x.CreationDateTime);
        }

        public Query BuildDelete(Guid id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Query BuildGetCount()
        {
            return new Query(TABLE_NAME)
                .SelectRaw("count(*) as count");
        }

        public Query BuildInsert(TestParentModel entity)
        {
            var data = new List<object[]>
            {
                new object[]
                {
                    entity.Id,
                    entity.Name,
                    entity.CreationDateTime
                }
            };

            return new Query(TABLE_NAME)
                .AsInsert(ALL_COLUMNS, data);
        }

        public Query BuildInsertMany(List<TestParentModel> entities)
        {
            throw new NotImplementedException();
        }

        public Query BuildUpdate(Guid id, TestParentModel newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
