using System.Collections.Generic;

namespace Ao.Stock.IntangibleProviders
{
    public class SqliteSQLIntangibleProvider : SQLIntangibleProvider
    {
        public const string MemoryHost = ":memory:";

        public const string ModeKey = "sqlite.Mode";

        public const string CacheKey = "sqlite.Cache";

        public const string PoolingKey = "sqlite.Pooling";

        public static readonly SqliteSQLIntangibleProvider Default = new SqliteSQLIntangibleProvider();

        private static readonly HashSet<string> keys = new HashSet<string>
        {
            HostKey,ModeKey,CacheKey,PoolingKey
        };

        public override IEnumerable<string> GetKeys()
        {
            return keys;
        }
        public override bool TryInverseReplace(string key, out string? result)
        {
            result = key;
            switch (key)
            {
                case "Source":
                    result = HostKey;
                    break;
                case "Password":
                    result = PasswordKey;
                    break;
                case "Mode":
                    result = ModeKey;
                    break;
                case "Cache":
                    result = CacheKey;
                    break;
                case "Pooling":
                    result = PoolingKey;
                    break;
                case "Connect Timeout":
                    result = ConnectTimeoutKey;
                    break;
                default:
                    break;
            }
            return true;
        }

        public override bool TryReplace(string key, out string? result)
        {
            result = key;
            switch (key)
            {
                case HostKey:
                    result = "Source";
                    break;
                case PasswordKey:
                    result = "Password";
                    break;
                case ModeKey:
                    result = "Mode";
                    break;
                case PoolingKey:
                    result = "Pooling";
                    break;
                case CacheKey:
                    result = "Cache";
                    break;
                case ConnectTimeoutKey:
                    result = "Connect Timeout";
                    break;
                default:
                    break;
            }
            return true;
        }
        public override string MakeString(IEnumerable<string> keys, IEnumerable<string> values, IntangibleProviderJoinOptions options)
        {
            return "Data " + base.MakeString(keys, values, options);
        }
    }

}
