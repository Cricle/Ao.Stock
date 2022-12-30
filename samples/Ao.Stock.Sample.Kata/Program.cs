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
                        new MethodMetadata(KnowsMethods.StrConcat,
                            new MethodMetadata(
                                KnowsMethods.Year,
                                new ValueMetadata("记录时间",true)),
                            new ValueMetadata("-"),
                            new MethodMetadata(
                                KnowsMethods.Weak,
                                new ValueMetadata("记录时间",true),
                                new ValueMetadata(0))),"aaaa"))
            };
            var q = new Query().From("1d85aeb8-046a-4619-902d-799d09b34bd4");
            var sql = KataMetadataVisitor.Mysql(q).VisitAndCompile(m, q);
            Console.WriteLine(sql);
        }
    }

}