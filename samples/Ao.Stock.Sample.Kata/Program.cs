using Ao.Stock.Kata.Copying;
using Ao.Stock.Mirror;
using Ao.Stock.Querying;
using Ao.Stock.Warehouse;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using DatabaseSchemaReader.SqlGen;
using MySqlConnector;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text;

namespace Ao.Stock.Sample.Kata
{
    internal class Program
    {
        static Task Main(string[] args)
        {
            return GenericBackupScriptAsync();
        }
        private static async Task GenericBackupScriptAsync()
        {
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
            using (var conn = new MySqlConnection(mysql))
            {
                using (var fi = File.Create("a.sql"))
                using (var sw = new StreamWriter(fi))
                {
                    var swx = Stopwatch.GetTimestamp();
                    var dbReader = new DatabaseReader(conn) { Owner = "sakila" };
                    dbReader.AllTables();
                    foreach (var item in dbReader.DatabaseSchema.Tables)
                    {
                        item.SchemaOwner = "sakila1";
                    }
                    var ddlFactory = new DdlGeneratorFactory(SqlType.MySql);
                    sw.WriteLine(SQLDatabaseCreateAdapter.MySql.GenericCreateIfNotExistsSql("sakila1"));
                    sw.WriteLine(ddlFactory.AllTablesGenerator(dbReader.DatabaseSchema).Write());
                    foreach (var item in dbReader.DatabaseSchema.Tables)
                    {
                        var wrapper = DefaultMethodWrapper.MySql;
                        var b = DelegateSQLBackup.TextWriter(sw, DefaultMethodWrapper.MySql, item.Name, "sakila1");
                        var tb = wrapper.Quto("sakila") + "." + wrapper.Quto(item.Name);
                        using (var comm = conn.CreateCommand($"SELECT * FROM {tb}"))
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
                var source = new DelegateSQLDatabaseInfo("sakila", conn, DefaultMethodWrapper.MySql);
                var dest = new DelegateSQLDatabaseInfo("sakila1", conn1, DefaultMethodWrapper.MySql);
                var copy = new SQLCognateCopying(source, dest) { TableFilter = new string[] { "store", "student" } };
                copy.SynchronousStructure = true;
                copy.WithDelete = true;
                var sw = Stopwatch.GetTimestamp();
                await copy.RunAsync();
                Console.WriteLine(Stopwatch.GetElapsedTime(sw));
            }

        }

    }
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public class Student
    {
        public string Name { get; set; }

        public long Id { get; set; }

        public override string ToString()
        {
            return $"{Name}, {Id}";
        }
    }
}