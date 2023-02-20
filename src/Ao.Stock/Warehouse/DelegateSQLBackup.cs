using Ao.Stock.Querying;
using System;
using System.IO;
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
        public static DelegateSQLBackup AsyncTextWriter(TextWriter textWriter, IMethodWrapper methodWrapper, string table, string? database = null)
        {
            return new DelegateSQLBackup(t => textWriter.WriteLineAsync(t), methodWrapper, table, database);
        }
        public static DelegateSQLBackup TextWriter(TextWriter textWriter, IMethodWrapper methodWrapper, string table, string? database = null)
        {
            return new DelegateSQLBackup(t =>
            {
                textWriter.WriteLine(t);
                return Task.CompletedTask;
            }, methodWrapper, table, database);
        }
        public static DelegateSQLBackup StringBuilder(StringBuilder stringBuilder, IMethodWrapper methodWrapper, string table, string? database = null)
        {
            return new DelegateSQLBackup(t =>
            {
                stringBuilder.AppendLine(t);
                return Task.CompletedTask;
            }, methodWrapper, table, database);
        }
    }
}
