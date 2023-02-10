using Ao.Stock.IntangibleProviders;
using Ao.Stock.Kata;
using Ao.Stock.Kata.Copying;
using Ao.Stock.SQL;
using Ao.Stock.SQL.MySql;
using System.Diagnostics;

namespace Ao.Stock.Sample.Scal
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var targetDb = "123";
            var ctxSource = SQLIntangibleContextHelper.MySql(new IntangibleContext
            {
                [SQLIntangibleProvider.HostKey] = "127.0.0.1",
                [SQLIntangibleProvider.PortKey] = 3306,
                [SQLIntangibleProvider.UserNameKey] = "root",
                [SQLIntangibleProvider.PasswordKey] = "",
                [SQLIntangibleProvider.DatabaseKey] = targetDb,
                ["characterset"] = "utf8mb4"
            });
            var ctxDest = SQLIntangibleContextHelper.MySql($"Server=127.0.0.1;Port=3306;UId=root;Pwd=;Database={targetDb}1;characterset=utf8mb4;");
            var source = new DelegateSQLDatabaseInfo(targetDb,
                ctxSource,
                MySqlStockIntangible.Default,
                CompilerFetcher.Mysql);
            var dest = new DelegateSQLDatabaseInfo(targetDb + 1,
                ctxDest,
                MySqlStockIntangible.Default,
                CompilerFetcher.Mysql);
            var copying = new SQLCognateCopying(source, dest) { SynchronousStructure = false, SynchronousStructureWithDelete = false, CleanTable = true };
            var sw = Stopwatch.GetTimestamp();
            await copying.RunAsync();
            Console.WriteLine(new TimeSpan(Stopwatch.GetTimestamp() - sw));
        }
    }
}