using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Ao.Stock
{
    public abstract class SQLArchitectureQuerying : IArchitectureQuerying
    {
        protected SQLArchitectureQuerying(DbConnection dbConnection)
        {
            DbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public DbConnection DbConnection { get; }

        protected virtual async Task EnsureOpenAsync()
        {
            if (DbConnection.State != ConnectionState.Open)
            {
                await DbConnection.OpenAsync();
            }
        }

        public async Task<IList<string>> GetDatabasesAsync()
        {
            await EnsureOpenAsync();
            using (var comm = GetGetDatabasesCommand())
            {
                using (var reader = await comm.ExecuteReaderAsync())
                {
                    return await ReadDataBasesAsync(reader);
                }
            }
        }
        protected Task<IList<string>> ReadAsync(DbDataReader reader)
        {
            var ds = new List<string>();
            while (reader.Read())
            {
                ds.Add(reader[0].ToString());
            }
            return Task.FromResult<IList<string>>(ds);
        }
        protected Task<IList<string>> ReadDataBasesAsync(DbDataReader reader)
        {
            return ReadAsync(reader);
        }

        protected abstract DbCommand GetGetDatabasesCommand();

        public async Task<IList<string>> GetTablesAsync(string database, IIntangibleContext context)
        {
            await EnsureOpenAsync();
            using (var comm = GetGetTablesCommand(database, context))
            {
                using (var reader = await comm.ExecuteReaderAsync())
                {
                    return await ReadTablesAsync(reader);
                }
            }
        }

        protected abstract DbCommand GetGetTablesCommand(string database, IIntangibleContext context);

        protected Task<IList<string>> ReadTablesAsync(DbDataReader reader)
        {
            return ReadAsync(reader);
        }
    }

}
