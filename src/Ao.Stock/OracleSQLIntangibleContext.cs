using System.Collections.Generic;

namespace Ao.Stock
{
    public class OracleSQLIntangibleContext : SQLIntangibleContext
    {
        public const string IntegratedSecurityKey = "oracle.IntegratedSecurity";

        public OracleSQLIntangibleContext()
        {
            Host = "127.0.0.1";
        }

        public string IntegratedSecurity
        {
            get => this.GetOrDefault<string>(IntegratedSecurityKey);
            set => this[IntegratedSecurityKey] = value;
        }
        protected override bool IsNotOthers(KeyValuePair<object, object> input)
        {
            return Equals(input.Key, IntegratedSecurityKey);
        }
        public override string ToString()
        {
            return NoContainsIgnore(Host, "Data Source")+
                NoContainsIgnore(UserNameKey, "User Id") +
                NoContainsIgnore(PasswordKey, "Password") +
                NoContainsIgnore(IntegratedSecurityKey, "Integrated Security") +
                NoContainsIgnore(ConnectTimeoutKey, "Connection Timeout") +
                JoinOthers();
        }
        public OracleSQLIntangibleContext SetIntegratedSecurity(bool enable)
        {
            IntegratedSecurity = enable ? "yes" : "no";
            return this;
        }
    }
}
