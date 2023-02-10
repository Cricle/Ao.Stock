using System.Collections.Generic;

namespace Ao.Stock.IntangibleProviders
{
    public class SqlServerSQLIntangibleProvider : SQLIntangibleProvider
    {
        public const string IntegratedSecurityKey = "sqlserver.IntegratedSecurity";

        public static readonly SqlServerSQLIntangibleProvider Default = new SqlServerSQLIntangibleProvider();

        private static readonly HashSet<string> keys = GetKnowKeysWith(new[] { IntegratedSecurityKey });

        public override IEnumerable<string> GetKeys()
        {
            return keys;
        }
        public override bool TryInverseReplace(string key, out string? result)
        {
            result = key;
            switch (key)
            {
                case "Source":
                    result = HostKey;
                    break;
                case "User Id":
                    result = UserNameKey;
                    break;
                case "Password":
                    result = PasswordKey;
                    break;
                case "Initial Catalog":
                    result = DatabaseKey;
                    break;
                case "Integrated Security":
                    result = IntegratedSecurityKey;
                    break;
                case "Connect Timeout":
                    result = ConnectTimeoutKey;
                    break;
                default:
                    break;
            }
            return true;
        }
        public override bool TryReplace(string key, out string? result)
        {
            result = key;
            switch (key)
            {
                case HostKey:
                    result = "Source";
                    break;
                case UserNameKey:
                    result = "User Id";
                    break;
                case PasswordKey:
                    result = "Password";
                    break;
                case DatabaseKey:
                    result = "Initial Catalog";
                    break;
                case IntegratedSecurityKey:
                    result = "Integrated Security";
                    break;
                case ConnectTimeoutKey:
                    result = "Connect Timeout";
                    break;
                default:
                    break;
            }
            return true;
        }
        public override string MakeString(IEnumerable<string> keys, IEnumerable<string> values, IntangibleProviderJoinOptions options)
        {
            return "Data " + base.MakeString(keys, values, options);
        }
    }

}
