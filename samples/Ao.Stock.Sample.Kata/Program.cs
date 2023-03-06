using Ao.Stock.Kata;
using Ao.Stock.Kata.Copying;
using Ao.Stock.Mirror;
using Ao.Stock.Querying;
using Ao.Stock.Warehouse;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using DatabaseSchemaReader.SqlGen;
using MySqlConnector;
using SqlKata;
using SqlKata.Compilers;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace Ao.Stock.Sample.Kata
{
    internal class Program
    {
        static Task Main(string[] args)
        {
            return BuildQueryAsync();
        }
        private static async Task ReadDataAsync()
        {
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
            using (var conn = new MySqlConnection(mysql))
            {
                var res = await conn.ExecuteReaderAsync<Student>("SELECT * FROM `student` LIMIT 10");
                foreach (var item in res)
                {
                    Console.WriteLine(item);
                }
            }
        }
        private static async Task GenericBackupScriptAsync()
        {
            const string tb = "sakila";
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database={tb};characterset=utf8mb4;";
            using (var conn = new MySqlConnection(mysql))
            {
                using (var fi = File.Create("a.sql"))
                using (var sw = new StreamWriter(fi))
                {
                    var swx = Stopwatch.GetTimestamp();
                    var dbReader = new DatabaseReader(conn) { Owner = tb };
                    dbReader.AllTables();
                    var ddlFactory = new DdlGeneratorFactory(SqlType.MySql);
                    sw.WriteLine(SQLDatabaseCreateAdapter.MySql.GenericCreateDatabaseIfNotExistsSql(tb));
                    sw.WriteLine(ddlFactory.AllTablesGenerator(dbReader.DatabaseSchema).Write());
                    foreach (var item in dbReader.DatabaseSchema.Tables)
                    {
                        var wrapper = DefaultMethodWrapper.MySql;
                        var b = DelegateSQLBackup.AsyncTextWriter(sw, DefaultMethodWrapper.MySql, item.Name, tb);
                        var tbx = wrapper.Quto(tb) + "." + wrapper.Quto(item.Name);
                        using (var comm = conn.CreateCommand($"SELECT * FROM {tbx}"))
                        using (var reader = comm.ExecuteReader())
                        {
                            await b.ConvertAsync(reader);
                        }
                    }
                    Console.WriteLine(Stopwatch.GetElapsedTime(swx));
                }
            }
        }
        private static async Task RunBackupAsync()
        {
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
            using (var conn = new MySqlConnection(mysql))
            {
                var s = new StringBuilder();
                var b = DelegateSQLBackup.StringBuilder(s, DefaultMethodWrapper.MySql, "address");
                using (var comm = conn.CreateCommand("SELECT * FROM address"))
                using (var reader = comm.ExecuteReader())
                {
                    await b.ConvertAsync(reader);
                    Console.WriteLine(s.ToString());
                }
            }
        }
        private static async Task RunSyncAsync()
        {
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
            var mysql1 = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila1;characterset=utf8mb4;";
            using (var conn = new MySqlConnection(mysql))
            using (var conn1 = new MySqlConnection(mysql1))
            {
                conn.Open();
                conn1.Open();
                var source = new DelegateSQLDatabaseInfo("sakila", conn, DefaultMethodWrapper.MySql);
                var dest = new DelegateSQLDatabaseInfo("sakila1", conn1, DefaultMethodWrapper.MySql);
                var copy = new SQLCopying(source, dest) { TableFilter = new string[] { "store", "student" } };
                copy.SynchronousStructure = true;
                copy.WithDelete = true;
                var sw = Stopwatch.GetTimestamp();
                await copy.RunAsync();
                Console.WriteLine(Stopwatch.GetElapsedTime(sw));
            }
        }
        private static Task BuildQueryAsync()
        {
            var whereInValues = Enumerable.Range(0, 10).Select(x => new ValueMetadata(x));
            var whereIn = new[] { new ValueMetadata("address_id", true) }.Concat(whereInValues).ToArray();
            var m = new MultipleQueryMetadata
            {
                new SelectMetadata(
                    new RawMetadata("a.bbbbb"),
                    new ValueMetadata("name",true),
                    new ValueMetadata("store_id",true),
                    new AliasMetadata(
                        new MethodMetadata(KnowsMethods.Abs,
                            new MethodMetadata(KnowsMethods.Power,
                                new ValueMetadata("store_id",true),
                                new ValueMetadata(2))),"store_id_tmp")),
                new FilterMetadata(
                    new BinaryMetadata(
                        new MethodMetadata(KnowsMethods.Like,new ValueMetadata("name",true),new ValueMetadata("%hell%")),
                        ExpressionType.OrElse,
                        new MethodMetadata(KnowsMethods.In,whereIn))),
                new LimitMetadata(10)
            };
            var query = new Query().From("store1");
            var visitor = KataMetadataVisitor.Mysql(query);
            var res = visitor.VisitAndCompile(m, query);
            Console.WriteLine(res);
            return Task.CompletedTask;
        }
    }
    public record class Student
    {
        public string Name1 { get; set; }

        public long Id { get; set; }

        public override string ToString()
        {
            return $"{Name1}, {Id}";
        }
    }
}