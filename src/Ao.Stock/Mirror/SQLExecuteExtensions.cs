using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
             int? commandTimeout = null,
            CancellationToken token = default)
        {
            using (var command = CreateCommand(connection, sql, args))
            {
                if (commandTimeout!=null)
                {
                    command.CommandTimeout = commandTimeout.Value;
                }
                using (var reader = await command.ExecuteReaderAsync(token))
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
        }
        public static Task<List<IDictionary<string, object>>> ExecuteReaderAsync(this DbConnection connection,
            string sql,
            IEnumerable<KeyValuePair<string, object>>? args = null,
             int? commandTimeout = null,
            CancellationToken token = default)
        {
            return ExecuteReaderAsync(connection, sql, DictionaryReaderAsyncConverter.Instance, args, commandTimeout, token);
        }
        public static Task<List<T>> ExecuteReaderAsync<T>(this DbConnection connection,
            string sql,
            IEnumerable<KeyValuePair<string, object>>? args = null,
            int? commandTimeout = null,
            CancellationToken token = default)
        {
            return ExecuteReaderAsync(connection, sql, ORMAsyncConverter<T>.Default, args,commandTimeout, token);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DbCommand CreateCommand(this DbConnection connection,
            string sql,
            IEnumerable<KeyValuePair<string, object>>? args = null)
        {
            EnsureConnectionOpen(connection);
            var comm = connection.CreateCommand();
            FillCommand(comm, sql, args);
            comm.Prepare();
            return comm;
        }
        public static async Task<TOutput> ExecuteReaderAsync<TOutput>(this DbConnection connection,
            string sql,
            IAsyncConverter<IDataReader, TOutput> converter,
            IEnumerable<KeyValuePair<string, object>>? args = null,
             int? commandTimeout = null,
            CancellationToken token = default)
        {
            using (var command = CreateCommand(connection, sql, args))
            {
                if (commandTimeout != null)
                {
                    command.CommandTimeout = commandTimeout.Value;
                }
                using (var reader = await command.ExecuteReaderAsync(token))
                {
                    return await converter.ConvertAsync(reader, token);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureConnectionOpen(DbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DbParameter CreateParamter(this DbCommand comm, string name, object value)
        {
            var par = comm.CreateParameter();
            par.ParameterName = name;
            par.Value = value;
            par.DbType = GetDbType(value);
            return par;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FillCommand(DbCommand comm, string sql, IEnumerable<KeyValuePair<string, object>>? args = null)
        {
            comm.CommandText = sql;
            if (args != null && args.Any())
            {
                foreach (var arg in args)
                {
                    comm.Parameters.Add(CreateParamter(comm, arg.Key, arg.Value));
                }
            }
        }
        public static async Task<int> ExecuteNonQueryAsync(this DbConnection connection, string sql, IEnumerable<KeyValuePair<string, object>>? args = null, int? timeout = null, CancellationToken token = default)
        {
            using (var command = CreateCommand(connection, sql, args))
            {
                if (timeout!=null)
                {
                    command.CommandTimeout = timeout.Value;
                }
                return await command.ExecuteNonQueryAsync(token).ConfigureAwait(false);
            }
        }
        public static DbType GetDbType(object? item)
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
