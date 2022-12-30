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
            get => GetOrDefault<bool>(IntegratedSecurityKey);
            set => this[IntegratedSecurityKey] = value;
        }

        protected override bool IsNotOthers(KeyValuePair<object, object> input)
        {
            return Equals(input.Key ,IntegratedSecurityKey);
        }

        public override string ToString()
        {
            return $"Data Source={Host},{Port};" +
                NoContainsIgnore(UserNameKey, "User Id") +
                NoContainsIgnore(PasswordKey, "Password") +
                NoContainsIgnore(DatabaseKey, "Initial Catalog") +
                NoContainsIgnore(IntegratedSecurityKey, "Integrated Security") +
                NoContainsIgnore(ConnectTimeoutKey, "Connection Timeout")+
                JoinOthers();
        }
    }
}
