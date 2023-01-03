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
            var connCtx = new MySqlSQLIntangibleContext
            {
                Database="sakila",
                UserName="root"
            };
            Console.WriteLine(connCtx.ToString());
            var conn = MySqlStockIntangible.Default.Get<DbConnection>(connCtx);
            conn.Open();
            var query=DelegateSQLArchitectureQuerying.Mysql(conn);
            for (int i = 0; i < 10; i++)
            {
                var sw = Stopwatch.GetTimestamp();
                var res=await query.GetDatabasesAsync();
                foreach (var item in res)
                {
                    var tables = await query.GetTablesAsync(item, connCtx);
                }
                Console.WriteLine(new TimeSpan(Stopwatch.GetTimestamp()-sw));
            }
        }
    }
}