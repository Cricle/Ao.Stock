using Ao.Stock.Mirror;
using MySqlConnector;

namespace Ao.Stock.Sample.Kata
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
            using (var conn=new MySqlConnection(mysql))
            {
                var stus =await conn.ExecuteReaderAsync<Student>("SELECT * FROM `student`");
                foreach (var item in stus)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
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