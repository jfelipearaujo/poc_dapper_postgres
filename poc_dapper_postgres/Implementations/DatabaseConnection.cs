using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapper;

using Npgsql;

using poc_dapper_postgres.Interfaces;

using PostgreSQLCopyHelper;

using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace poc_dapper_postgres.Implementations
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly NpgsqlConnection connection;
        private readonly QueryFactory queryFactory;

        public DatabaseConnection(NpgsqlConnection connection)
        {
            this.connection = connection;

            queryFactory = new QueryFactory(this.connection, new PostgresCompiler());
        }

        public void Dispose()
        {
            connection?.Dispose();
            queryFactory?.Dispose();

            GC.SuppressFinalize(this);
        }

        public async Task<int> ExecuteRawQueryAsync(string query)
        {
            return await connection.ExecuteAsync(query);
        }

        public async Task<int> ExecuteAsync(Query query)
        {
            return await queryFactory.ExecuteAsync(query);
        }

        public async Task<int> InsertAsync(Query query)
        {
            return await ExecuteAsync(query);
        }

        public async Task<ulong> BulkInsert<T>(PostgreSQLCopyHelper<T> copyHelper, IEnumerable<T> entities)
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            return await copyHelper.SaveAllAsync(connection, entities);
        }

        public async Task<T> GetOneAsync<T>(Query query)
        {
            return await queryFactory.FirstOrDefaultAsync<T>(query);
        }

        public async Task<IEnumerable<T>> GetAsync<T>(Query query)
        {
            return await queryFactory.GetAsync<T>(query);
        }

        public async Task<int> UpdateAsync(Query query)
        {
            return await ExecuteAsync(query);
        }

        public async Task<int> DeleteAsync(Query query)
        {
            return await ExecuteAsync(query);
        }
    }
}
