using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public static class SQLExecuteExtensions
    {
        public static async Task<int> ExecuteReadCountAsync(this DbConnection connection,
            string sql,
            IEnumerable<KeyValuePair<string, object>>? args = null,
            CancellationToken token = default)
        {
            await EnsureConnectionOpenAsync(connection, token).ConfigureAwait(false);
            using (var comm = connection.CreateCommand())
            {
                FillCommand(comm, sql, args);
                comm.Prepare();
                using (var reader = await comm.ExecuteReaderAsync(token).ConfigureAwait(false))
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
        }
        public static Task<List<IDictionary<string, object>>> ExecuteReaderAsync(this DbConnection connection,
            string sql,
            IEnumerable<KeyValuePair<string, object>>? args = null,
            CancellationToken token = default)
        {
            return ExecuteReaderAsync(connection, sql, DictionaryReaderAsyncConverter.Instance, args, token);
        }
        public static async Task<TOutput> ExecuteReaderAsync<TOutput>(this DbConnection connection,
            string sql, 
            IAsyncConverter<DbDataReader,TOutput> converter,
            IEnumerable<KeyValuePair<string, object>>? args = null, 
            CancellationToken token = default)
        {
            await EnsureConnectionOpenAsync(connection, token).ConfigureAwait(false);
            using (var comm = connection.CreateCommand())
            {
                FillCommand(comm, sql, args);
                comm.Prepare();
                using (var reader = await comm.ExecuteReaderAsync(token).ConfigureAwait(false))
                {
                    var output = await converter.ConvertAsync(reader, token);
                    return output;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Task EnsureConnectionOpenAsync(DbConnection connection, CancellationToken token = default)
        {
            if (connection.State != ConnectionState.Open)
            {
                return connection.OpenAsync(token);
            }
            return Task.CompletedTask;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FillCommand(DbCommand comm, string sql, IEnumerable<KeyValuePair<string, object>>? args = null)
        {
            comm.CommandText = sql;
            if (args != null && args.Any())
            {
                foreach (var arg in args)
                {
                    var par = comm.CreateParameter();
                    par.Value = arg;
                    par.DbType = GetDbType(arg.Value);
                    comm.Parameters.Add(par);
                }
            }
        }
        public static async Task<int> ExecuteNoQueryAsync(this DbConnection connection, string sql, IEnumerable<KeyValuePair<string, object>>? args = null, CancellationToken token = default)
        {
            await EnsureConnectionOpenAsync(connection, token).ConfigureAwait(false);
            using (var comm = connection.CreateCommand())
            {
                FillCommand(comm, sql, args);
                comm.Prepare();
                return await comm.ExecuteNonQueryAsync(token).ConfigureAwait(false);
            }
        }
        private static DbType GetDbType(object? item)
        {
            var typeCode = Convert.GetTypeCode(item);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return DbType.Boolean;
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return DbType.Byte;
                case TypeCode.Int16:
                    return DbType.Int16;
                case TypeCode.UInt16:
                    return DbType.UInt16;
                case TypeCode.Int32:
                    return DbType.Int32;
                case TypeCode.UInt32:
                    return DbType.UInt32;
                case TypeCode.Int64:
                    return DbType.Int64;
                case TypeCode.UInt64:
                    return DbType.UInt64;
                case TypeCode.Single:
                    return DbType.Single;
                case TypeCode.Double:
                    return DbType.Double;
                case TypeCode.Decimal:
                    return DbType.Decimal;
                case TypeCode.DateTime:
                    return DbType.DateTime;
                case TypeCode.String:
                    return DbType.String;
                default:
                    break;
            }
            if (item != null)
            {
                var itemType = item.GetType();
                if (itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return GetDbType(itemType.GetGenericArguments()[0]);
                }
            }
            return DbType.Object;
        }

    }
}
