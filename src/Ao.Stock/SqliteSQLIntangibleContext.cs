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
        protected override bool IsNotOthers(KeyValuePair<object, object> input)
        {
            return Equals(input.Key, ModeKey) ||
                Equals(input.Key, CacheKey) ||
                Equals(input.Key, PoolingKey);
        }

        public override string ToString()
        {
            return $"Data Source={Host}" +
                NoContainsIgnore(PasswordKey, "Password") +
                NoContainsIgnore(ModeKey, "Mode") +
                NoContainsIgnore(PoolingKey, "Pooling") +
                NoContainsIgnore(CacheKey, "Cache") +
                NoContainsIgnore(ConnectTimeoutKey, "Default Timeout") +
                JoinOthers();
        }
    }
}
