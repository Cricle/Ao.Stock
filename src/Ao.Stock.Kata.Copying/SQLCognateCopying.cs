using Ao.Stock.Mirror;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Kata.Copying
{
    public class SQLCognateCopying : SQLCopying
    {
        public SQLCognateCopying(ISQLDatabaseInfo source, ISQLDatabaseInfo destination) 
            : base(source, destination)
        {
        }

        protected virtual string GenerateQuerySql(ISQLDatabaseInfo info,string tableName)
        {
            return $"SELECT * FROM {info.CreateFullName(tableName)}";
        }

        protected override async Task CopyAsync(DbConnection sourceConn, DbConnection destConn, IEnumerable<string> tables, CancellationToken token)
        {
            foreach (var item in tables)
            {
                token.ThrowIfCancellationRequested();
                var sql= GenerateQuerySql(Source,item);
                var cp = new SQLCognateMirrorCopy(destConn,
                    sql,
                    Destination.CreateFullName(item));
                await cp.CopyAsync();
            }
        }
    }

}
