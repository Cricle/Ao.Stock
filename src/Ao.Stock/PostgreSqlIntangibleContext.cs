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
            get => this.GetOrDefault<bool>(IntegratedSecurityKey);
            set => this[IntegratedSecurityKey] = value;
        }

        public bool Pooling
        {
            get => this.GetOrDefault<bool>(PoolingKey);
            set => this[PoolingKey] = value;
        }
        public int MinPoolSize
        {
            get => this.GetOrDefault<int>(MinPoolSizeKey);
            set => this[MinPoolSizeKey] = value;
        }
        public int MaxPoolSize
        {
            get => this.GetOrDefault<int>(MaxPoolSizeKey);
            set => this[MaxPoolSizeKey] = value;
        }
        protected override bool IsOthers(KeyValuePair<object, object> input)
        {
            return Equals(input.Key, IntegratedSecurityKey) ||
                Equals(input.Key, PoolingKey) ||
                Equals(input.Key, MinPoolSizeKey) ||
                Equals(input.Key, MaxPoolSizeKey);
        }
        public override string ReplaceKey(string key)
        {
            switch (key)
            {
                case HostKey:
                    return "Source";
                case PortKey:
                    return "Port";
                case UserNameKey:
                    return "User Id";
                case PasswordKey:
                    return "Password";
                case DatabaseKey:
                    return "Database";
                case IntegratedSecurityKey:
                    return "Integrated Security";
                case ConnectTimeoutKey:
                    return "CommandTimeout";
                case PoolingKey:
                    return "Pooling";
                case MinPoolSizeKey:
                    return "MinPoolSize";
                case MaxPoolSizeKey:
                    return "MaxPoolSize";
                default:
                    return key;
            }
        }
        public override string ToString()
        {
            return NoContainsIgnore(HostKey) +
                NoContainsIgnore(PortKey) +
                NoContainsIgnore(UserNameKey) +
                NoContainsIgnore(PasswordKey) +
                NoContainsIgnore(DatabaseKey) +
                NoContainsIgnore(IntegratedSecurityKey) +
                NoContainsIgnore(ConnectTimeoutKey) +
                NoContainsIgnore(PoolingKey) +
                NoContainsIgnore(MinPoolSizeKey) +
                NoContainsIgnore(MaxPoolSizeKey) +
                JoinOthers();
        }
    }
}
