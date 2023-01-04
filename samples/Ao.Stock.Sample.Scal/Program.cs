using Ao.Stock.Kata;
using Ao.Stock.Kata.Copying;
using Ao.Stock.SQL;
using Ao.Stock.SQL.MySql;
using FluentMigrator.Runner;
using System.Data.Common;
using System.Diagnostics;

namespace Ao.Stock.Sample.Scal
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var targetDb = "123";
            var ctxSource = new SQLIntangibleContext
            {
                [SQLStockIntangible.ConnectionStringKey] = $"server=192.168.1.100;port=3306;userid=root;password=;database={targetDb};characterset=utf8mb4;"
            };
            var ctxDest = new SQLIntangibleContext
            {
                [SQLStockIntangible.ConnectionStringKey] = $"server=192.168.1.100;port=3306;userid=root;password=;database={targetDb}1;characterset=utf8mb4;"
            };
            var source=new DelegateSQLDatabaseInfo(targetDb, 
                ctxSource,
                MySqlStockIntangible.Default,
                CompilerFetcher.Mysql);
            var dest = new DelegateSQLDatabaseInfo(targetDb+1,
                ctxDest,
                MySqlStockIntangible.Default,
                CompilerFetcher.Mysql);
            var copying = new SQLCopying(source, dest);
            var sw = Stopwatch.GetTimestamp();
            await copying.RunAsync();
            Console.WriteLine(new TimeSpan(Stopwatch.GetTimestamp()-sw));
        }
    }
}