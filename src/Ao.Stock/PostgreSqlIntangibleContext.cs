using System.Collections.Generic;

namespace Ao.Stock
{
    public class PostgreSqlIntangibleContext : SQLIntangibleContext
    {
        public PostgreSqlIntangibleContext()
        {
            Host = "127.0.0.1";
            Port = 5432;
        }

        public const string IntegratedSecurityKey = "postgresql.IntegratedSecurity";

        public const string PoolingKey = "postgresql.Pooling";

        public const string MinPoolSizeKey = "postgresql.MinPoolSize";

        public const string MaxPoolSizeKey = "postgresql.MaxPoolSize";
        
        public bool IntegratedSecurity
        {
            get => GetOrDefault<bool>(IntegratedSecurityKey);
            set => this[IntegratedSecurityKey] = value;
        }

        public bool Pooling
        {
            get => GetOrDefault<bool>(PoolingKey);
            set => this[PoolingKey] = value;
        }
        public int MinPoolSize
        {
            get => GetOrDefault<int>(MinPoolSizeKey);
            set => this[MinPoolSizeKey] = value;
        }
        public int MaxPoolSize
        {
            get => GetOrDefault<int>(MaxPoolSizeKey);
            set => this[MaxPoolSizeKey] = value;
        }
        protected override bool IsNotOthers(KeyValuePair<object, object> input)
        {
            return Equals(input.Key, IntegratedSecurityKey) ||
                Equals(input.Key, PoolingKey) ||
                Equals(input.Key, MinPoolSizeKey) ||
                Equals(input.Key, MaxPoolSizeKey);
        }
        public override string ToString()
        {
            return NoContainsIgnore(Host, "Server") +
                NoContainsIgnore(PortKey, "Port") +
                NoContainsIgnore(UserNameKey, "User Id") +
                NoContainsIgnore(PasswordKey, "Password") +
                NoContainsIgnore(DatabaseKey, "Database") +
                NoContainsIgnore(IntegratedSecurityKey, "Integrated Security") +
                NoContainsIgnore(ConnectTimeoutKey, "CommandTimeout") +
                NoContainsIgnore(PoolingKey, "Pooling") +
                NoContainsIgnore(MinPoolSizeKey, "MinPoolSize") +
                NoContainsIgnore(MaxPoolSizeKey, "MaxPoolSize") +
                JoinOthers();
        }
    }
}
