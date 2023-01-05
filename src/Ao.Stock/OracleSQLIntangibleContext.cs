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
                case IntegratedSecurityKey:
                    return "Integrated Security";
                case ConnectTimeoutKey:
                    return "Connection Timeout";
                default:
                    return key;
            }
        }
        public override string ToString()
        {
            return "Data"+ NoContainsIgnore(HostKey, "Source") +
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
