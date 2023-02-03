using Ao.Stock.IntangibleProviders;
using System;
using System.Linq;

namespace Ao.Stock
{
    public static class IntangiblesExtensions
    {
        public static string MakeString(this IIntangibleContext context, IIntangibleProvider provider, IntangibleProviderJoinOptions options)
        {
            return provider.MakeString(context.Keys.Select(x => x.ToString()), context.Values.Select(x => x.ToString()), options);
        }
        public static IIntangibleContext Split(this IIntangibleProvider provider,string source)
        {
            var sps = source.Split(new string[] { provider.Separator }, StringSplitOptions.RemoveEmptyEntries);
            var ctx = new IntangibleContext();
            foreach (var item in sps)
            {
                var first = item.IndexOf(provider.JoinSeparator);
                if (first == -1)
                {
                    continue;
                }
                var key=item.Substring(0, first);
                key = provider.TryInverseReplace(key, out var keyResult)&&keyResult!=null ? keyResult : key;
                var value = item.Substring(first + provider.JoinSeparator.Length);
                ctx[key] = value;
            }
            return ctx;
        }
        public static string Concat(this IIntangibleProvider provider, string source, string key, string value)
        {
            if (string.IsNullOrEmpty(source))
            {
                return provider.Concat(key, value);
            }
            if (!source.EndsWith(provider.Separator))
            {
                source += provider.Separator;
            }
            return source + provider.Concat(key, value);
        }
    }
}
