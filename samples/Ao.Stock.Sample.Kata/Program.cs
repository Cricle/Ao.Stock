using Ao.Stock.Mirror;
using DatabaseSchemaReader;
using DatabaseSchemaReader.Compare;
using DatabaseSchemaReader.DataSchema;
using DatabaseSchemaReader.SqlGen;
using MySqlConnector;
using System.Diagnostics.CodeAnalysis;

namespace Ao.Stock.Sample.Kata
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
            using (var conn=new MySqlConnection(mysql))
            {
                var reader=new DatabaseReader(conn);
                reader.Owner = "xassa";
                var sch=reader.ReadAll();
                reader.Owner = "sakila";
                var sh2 = reader.ReadAll();
                var fc = new DdlGeneratorFactory(SqlType.MySql);
                var stus = await conn.ExecuteReaderAsync<Student>("SELECT * FROM `student`");
                foreach (var item in stus)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
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