using System;
using System.Data.Common;

namespace Ao.Stock.Mirror
{
    public readonly struct SQLMirrorTarget : IDisposable
    {
        public SQLMirrorTarget(DbConnection connection, SQLTableInfo tableInfo)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            TableInfo = tableInfo;
        }

        public DbConnection Connection { get; }

        public SQLTableInfo TableInfo { get; }

        public void Dispose()
        {
            Connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
