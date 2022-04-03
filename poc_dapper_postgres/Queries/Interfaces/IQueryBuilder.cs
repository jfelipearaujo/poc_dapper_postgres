using System;
using System.Collections.Generic;

using PostgreSQLCopyHelper;

using SqlKata;

namespace poc_dapper_postgres.Queries.Interfaces
{
    public interface IQueryBuilder<T>
    {
        Query BuildInsert(T entity);
        Query BuildInsertMany(List<T> entities);
        PostgreSQLCopyHelper<T> BuildBulkCopyHelper();
        Query BuildGetCount();
        Query BuildGetById(Guid id);
        Query BuildGetAll();
        Query BuildUpdate(Guid id, T newEntity);
        Query BuildDelete(Guid id);
        Query BuildDeleteAll();
    }
}
