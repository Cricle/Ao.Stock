using Ao.Stock.Querying;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Stock.Warehouse
{
    public class DelegateSQLBackup : SQLBackup
    {
        public DelegateSQLBackup(Func<string, Task> writeFunc,IMethodWrapper methodWrapper, string table, string? database = null)
            : base(methodWrapper, table, database)
        {
            WriteFunc = writeFunc ?? throw new ArgumentNullException(nameof(writeFunc));
        }

        public Func<string,Task> WriteFunc { get; }

        protected override Task WriteAsync(string text)
        {
            return WriteFunc(text);
        }
        public static DelegateSQLBackup StringBuilder(StringBuilder stringBuilder,IMethodWrapper methodWrapper, string table, string? database = null)
        {
            return new DelegateSQLBackup(t =>
            {
                stringBuilder.AppendLine(t);
                return Task.CompletedTask;
            }, methodWrapper, table, database);
        }
    }
}
