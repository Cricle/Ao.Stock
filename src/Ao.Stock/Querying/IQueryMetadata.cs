using System.Collections.Generic;
using System.Text;

namespace Ao.Stock.Querying
{
    public static class QueryMetadataExtensions
    {
        public static IEnumerable<T> Find<T>(this IQueryMetadata metadata)
            where T : IQueryMetadata
        {
            if (metadata is T)
            {
                yield return (T)metadata;
            }
            foreach (var item in metadata.GetChildren())
            {
                foreach (var sub in Find<T>(item))
                {
                    yield return sub;
                }
            }
        }
    }
    public interface IQueryMetadata
    {
        string? ToString();

        void ToString(StringBuilder builder);

        IEnumerable<IQueryMetadata> GetChildren();
    }
}
