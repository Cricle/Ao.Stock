using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Stock.Explains
{
    public class ExplainResultSet<TResult> : List<TResult>
        where TResult: IExplainResult,new()
    {
        public ExplainResultSet()
        {
        }

        public ExplainResultSet(IEnumerable<TResult> collection) : base(collection)
        {
        }

        public ExplainResultSet(int capacity) : base(capacity)
        {
        }
        public static async Task<ExplainResultSet<TResult>> FromDbConnectionAsync(DbConnection connection, string sql, Action<DbCommand>? commandAction = null)
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                commandAction?.Invoke(command);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return FromReader(reader);
                }
            }
        }
        public static ExplainResultSet<TResult> FromDbConnection(IDbConnection connection,string sql,Action<IDbCommand>? commandAction=null)
        {
            if (connection.State!= ConnectionState.Open)
            {
                connection.Open();
            }
            using (var command=connection.CreateCommand())
            {
                command.CommandText = sql;
                commandAction?.Invoke(command);
                using (var reader=command.ExecuteReader())
                {
                    return FromReader(reader);
                }
            }
        }
        public static ExplainResultSet<TResult> FromReader(IDataReader reader)
        {
            var set = new ExplainResultSet<TResult>();
            while (reader.Read())
            {
                var result = new TResult();
                result.WithReader(reader);
                set.Add(result);
            }
            return set;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in this)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }
    }
}
