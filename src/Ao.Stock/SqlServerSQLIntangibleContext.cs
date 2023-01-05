using System.Collections.Generic;

namespace Ao.Stock
{
    public class SqlServerSQLIntangibleContext : SQLIntangibleContext
    {
        public const string IntegratedSecurityKey = "sqlserver.IntegratedSecurity";

        public SqlServerSQLIntangibleContext()
        {
            Host = ".";
            Port = 1433;
        }

        public bool IntegratedSecurity
        {
            get => this.GetOrDefault<bool>(IntegratedSecurityKey);
            set => this[IntegratedSecurityKey] = value;
        }

        protected override bool IsOthers(KeyValuePair<object, object> input)
        {
            return Equals(input.Key, IntegratedSecurityKey);
        }
        public override string ReplaceKey(string key)
        {
            switch (key)
            {
                case HostKey:
                    return "Source";
                case UserNameKey:
                    return "User Id";
                case PasswordKey:
                    return "Password";
                case DatabaseKey:
                    return "Initial Catalog";
                case IntegratedSecurityKey:
                    return "Integrated Security";
                case ConnectTimeoutKey:
                    return "Connect Timeout";
                default:
                    return key;
            }
        }

        public override string ToString()
        {
            return $"Data Source={Host},{Port};" +
                NoContainsIgnore(UserNameKey) +
                NoContainsIgnore(PasswordKey) +
                NoContainsIgnore(DatabaseKey) +
                NoContainsIgnore(IntegratedSecurityKey) +
                NoContainsIgnore(ConnectTimeoutKey) +
                JoinOthers();
        }
    }
}
