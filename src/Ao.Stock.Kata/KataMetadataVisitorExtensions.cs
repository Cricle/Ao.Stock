using Ao.Stock.Querying;
using SqlKata;

namespace Ao.Stock.Kata
{
    public static class KataMetadataVisitorExtensions
    {
        public static SqlResult VisitAndCompile(this KataMetadataVisitor visitor, IQueryMetadata metadata, Query query)
        {
            visitor.Visit(metadata, visitor.CreateContext(metadata));
            return visitor.Compiler.Compile(query);
        }
    }
}
