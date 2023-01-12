using System.Collections.Generic;

namespace Ao.Stock
{
    public class SqliteSQLIntangibleContext : SQLIntangibleContext
    {
        public const string MemoryHost = ":memory:";

        public const string ModeKey = "sqlite.Mode";

        public const string CacheKey = "sqlite.Cache";

        public const string PoolingKey = "sqlite.Pooling";

        public string Mode
        {
            get => this.GetOrDefault<string>(ModeKey);
            set => this[ModeKey] = value;
        }
        public string Cache
        {
            get => this.GetOrDefault<string>(CacheKey);
            set => this[CacheKey] = value;
        }
        public bool Pooling
        {
            get => this.GetOrDefault<bool>(PoolingKey);
            set => this[PoolingKey] = value;
        }
        protected override bool IsOthers(KeyValuePair<object, object> input)
        {
            return Equals(input.Key, ModeKey) ||
                Equals(input.Key, CacheKey) ||
                Equals(input.Key, PoolingKey);
        }
        public override string ReplaceKey(string key)
        {
            switch (key)
            {
                case HostKey:
                    return "Source";
                case PasswordKey:
                    return "Password";
                case ModeKey:
                    return "Mode";
                case PoolingKey:
                    return "Pooling";
                case CacheKey:
                    return "Cache";
                case ConnectTimeoutKey:
                    return "Connect Timeout";
                default:
                    return key;
            }
        }
        public override string ToString()
        {
            return $"Data Source={Host}" +
                NoContainsIgnore(PasswordKey) +
                NoContainsIgnore(ModeKey) +
                NoContainsIgnore(PoolingKey) +
                NoContainsIgnore(CacheKey) +
                NoContainsIgnore(ConnectTimeoutKey) +
                JoinOthers();
        }
    }
}
