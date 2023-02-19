using Ao.Stock.Kata;
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
            using (var conn = new MySqlConnection(mysql))
            {
                var q = new Query("student");
                var query = new KataMetadataVisitor(new MySqlCompiler(),q , SqlMethodTranslatorHelpers<Compiler>.Mysql());
                var sql = query.VisitAndCompile(new MultipleQueryMetadata
                {
                    new SelectMetadata(new IQueryMetadata[]
                    {
                        new AliasMetadata(new MethodMetadata(KnowsMethods.StrLen,new ValueMetadata("Name",true)),"namelen"),
                        new ValueMetadata("Id",true),
                    })
                }, q);
                Console.WriteLine(sql.ToString());
                var res =await conn.ExecuteReaderAsync(sql.ToString());
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