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

        public override string ToString()
        {
            return NoContainsIgnore(HostKey, "Server") +
                NoContainsIgnore(PortKey, "Port") +
                NoContainsIgnore(UserNameKey, "Uid") +
                NoContainsIgnore(PasswordKey, "Pwd") +
                NoContainsIgnore(DatabaseKey, "Database") +
                NoContainsIgnore(SslModeKey, "SslMode") +
                NoContainsIgnore(ConnectTimeoutKey, "Connect Timeout") +
                JoinOthers();
        }
    }

}
