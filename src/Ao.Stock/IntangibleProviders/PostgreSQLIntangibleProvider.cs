using System.Collections.Generic;

namespace Ao.Stock.IntangibleProviders
{
    public class PostgreSQLIntangibleProvider : SQLIntangibleProvider
    {
        public const string IntegratedSecurityKey = "postgresql.IntegratedSecurity";

        public static readonly PostgreSQLIntangibleProvider Default = new PostgreSQLIntangibleProvider();

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
                case "Port":
                    result = PortKey;
                    break;
                case "Password":
                    result = PasswordKey;
                    break;
                case "User Id":
                    result = UserNameKey;
                    break;
                case "Database":
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
                case PortKey:
                    result = "Port";
                    break;
                case UserNameKey:
                    result = "User Id";
                    break;
                case PasswordKey:
                    result = "Password";
                    break;
                case DatabaseKey:
                    result = "Database";
                    break;
                case IntegratedSecurityKey:
                    result = "Integrated Security";
                    break;
                case ConnectTimeoutKey:
                    result = "CommandTimeout";
                    break;
                default:
                    break;
            }
            return true;
        }
    }

}
