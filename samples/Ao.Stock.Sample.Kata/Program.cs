using Ao.Stock.Kata;
using Ao.Stock.Kata.Copying;
using Ao.Stock.Mirror;
using Ao.Stock.Querying;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using DatabaseSchemaReader.SqlGen;
using MySqlConnector;
using SqlKata;
using SqlKata.Compilers;
using System.Diagnostics.CodeAnalysis;

namespace Ao.Stock.Sample.Kata
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=355343;database=sakila;characterset=utf8mb4;";
            var mysql1 = $"server=127.0.0.1;port=3306;userid=root;password=355343;database=sakila1;characterset=utf8mb4;";
            using (var conn = new MySqlConnection(mysql))
            using (var conn1 = new MySqlConnection(mysql1))
            {
                var source = new DelegateSQLDatabaseInfo("sakila", conn, DefaultMethodWrapper.MySql);
                var dest = new DelegateSQLDatabaseInfo("sakila1", conn1, DefaultMethodWrapper.MySql);
                var copy = new SQLCognateCopying(source,dest);
                copy.SynchronousStructure = true;
                copy.WithDelete= true;
                await copy.RunAsync();
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