using System.Collections.Generic;

namespace Ao.Stock.IntangibleProviders
{
    public interface IIntangibleProvider
    {
        string Separator { get; }

        string JoinSeparator { get; }

        IEnumerable<string> GetKeys();

        bool TryReplace(string key, out string? result);

        bool TryInverseReplace(string key, out string? result);

        string Concat(string key, string? value);

        string Join(IEnumerable<string> keys, IEnumerable<string> values, IntangibleProviderJoinOptions options);

        string MakeString(IEnumerable<string> keys, IEnumerable<string> values, IntangibleProviderJoinOptions options);
    }

}
