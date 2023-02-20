using Ao.Stock.Kata.Copying;
using Ao.Stock.Mirror;
using Ao.Stock.Querying;
using Ao.Stock.Warehouse;
using MySqlConnector;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Ao.Stock.Sample.Kata
{
    internal class Program
    {
        static Task Main(string[] args)
        {
            return RunBackupAsync();
        }
        private static async Task RunBackupAsync()
        {
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
            using (var conn = new MySqlConnection(mysql))
            {
                var s = new StringBuilder();
                var b = DelegateSQLBackup.StringBuilder(s, DefaultMethodWrapper.MySql, "staff", "sakila");
                using (var comm = conn.CreateCommand("SELECT * FROM staff"))
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