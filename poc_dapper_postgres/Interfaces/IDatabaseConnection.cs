using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using PostgreSQLCopyHelper;

using SqlKata;

namespace poc_dapper_postgres.Interfaces
{
    public interface IDatabaseConnection : IDisposable
    {
        Task<int> ExecuteRawQueryAsync(string query);
        Task<int> ExecuteAsync(Query query);
        Task<int> InsertAsync(Query query);
        Task<ulong> BulkInsert<T>(PostgreSQLCopyHelper<T> copyHelper, IEnumerable<T> entities);
        Task<T> GetOneAsync<T>(Query query);
        Task<IEnumerable<T>> GetAsync<T>(Query query);
        Task<int> UpdateAsync(Query query);
        Task<int> DeleteAsync(Query query);
    }
}
