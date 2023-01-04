using Ao.Stock.Kata.Mirror;
using Ao.Stock.Mirror;
using System.Data.Common;

namespace Ao.Stock.Kata.Copying
{
    public class SQLCognateCopying : SQLCopying
    {
        public SQLCognateCopying(ISQLDatabaseInfo source, ISQLDatabaseInfo destination) : base(source, destination)
        {
        }
        protected override async Task CopyAsync(DbConnection sourceConn, DbConnection destConn, IEnumerable<string> tables, CancellationToken token)
        {
            foreach (var item in tables)
            {
                token.ThrowIfCancellationRequested();
                var cp = new SQLCognateMirrorCopy(sourceConn,
                    new SQLTableInfo(Source.Database, item),
                    new SQLTableInfo(Destination.Database, item),
                    Source.Compiler);
                await cp.CopyAsync(Source.Context);
            }
        }
    }
}
