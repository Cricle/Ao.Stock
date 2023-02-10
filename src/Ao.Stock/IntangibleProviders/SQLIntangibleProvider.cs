using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ao.Stock.IntangibleProviders
{
    public abstract class SQLIntangibleProvider : IIntangibleProvider
    {
        public const string HostKey = "conn.Host";

        public const string PortKey = "conn.Port";

        public const string UserNameKey = "conn.UserName";

        public const string PasswordKey = "conn.Password";

        public const string ConnectTimeoutKey = "conn.ConnectTimeout";

        public const string DatabaseKey = "conn.DefaultDatabase";

        public virtual string Separator { get; } = ";";

        public virtual string JoinSeparator { get; } = "=";

        protected static HashSet<string> GetKnowKeysWith(IEnumerable<string> keys)
        {
            var hs = new HashSet<string>
            {
                HostKey,PortKey,UserNameKey,PasswordKey,ConnectTimeoutKey,DatabaseKey
            };
            foreach (var key in keys)
            {
                hs.Add(key);
            }
            return hs;
        }

        public virtual string Concat(string key, string? value)
        {
            return key + JoinSeparator + value;
        }

        public abstract IEnumerable<string> GetKeys();

        public virtual string Join(IEnumerable<string> keys, IEnumerable<string> values, IntangibleProviderJoinOptions options)
        {
            var sb = new StringBuilder();
            using (var enuKey = keys.GetEnumerator())
            using (var enuValue = values.GetEnumerator())
            {
                var supportKeys = GetKeys();
                while (enuKey.MoveNext() && enuValue.MoveNext())
                {
                    if (options.IgnoreNoSupport && !supportKeys.Contains(enuKey.Current))
                    {
                        continue;
                    }
                    var key = TryReplace(enuKey.Current, out var keyResult) ? keyResult : enuKey.Current;
                    sb.Append(Concat(key, enuValue.Current));
                    sb.Append(Separator);
                }
            }
            return sb.ToString();
        }

        public virtual string MakeString(IEnumerable<string> keys, IEnumerable<string> values, IntangibleProviderJoinOptions options)
        {
            return Join(keys, values, options);
        }

        public abstract bool TryInverseReplace(string key, out string? result);
        public abstract bool TryReplace(string key, out string? result);

    }

}
