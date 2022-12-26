using Ao.Stock.Kata;
using Ao.Stock.Querying;
using SqlKata;

namespace Ao.Stock.Sample.Kata
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var m = new MultipleQueryMetadata
            {
                new SelectMetadata(
                    new AliasMetadata(
                        new MethodMetadata(
                            KnowsMethods.Hour,
                            new ValueMetadata("a4",true)),"aaaa"))
            };
            var q = new Query().From("staff");
            var sql = KataMetadataVisitor.Sqlite(q).VisitAndCompile(m,q);
            Console.WriteLine(sql);
        }
    }

}