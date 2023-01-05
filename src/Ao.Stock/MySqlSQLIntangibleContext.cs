using System.Collections.Generic;

namespace Ao.Stock
{
    public class MySqlSQLIntangibleContext : SQLIntangibleContext
    {
        public const string SslModeKey = "mysql.SslMode";

        public MySqlSQLIntangibleContext()
        {
            Host = "127.0.0.1";
            Port = 3306;
        }

        public string SslMode
        {
            get => this.GetOrDefault<string>(SslModeKey);
            set => this[SslModeKey] = value;
        }

        protected override bool IsOthers(KeyValuePair<object, object> input)
        {
            return Equals(input.Key, SslModeKey);
        }
        public override string ReplaceKey(string key)
        {
            switch (key)
            {
                case HostKey:
                    return "Server";
                case PortKey:
                    return "Port";
                case UserNameKey:
                    return "Uid";
                case PasswordKey:
                    return "Pwd";
                case DatabaseKey:
                    return "Database";
                case SslModeKey:
                    return "SslMode";
                case ConnectTimeoutKey:
                    return "Connect Timeout";
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
                NoContainsIgnore(SslModeKey) +
                NoContainsIgnore(ConnectTimeoutKey) +
                JoinOthers();
        }
    }

}
