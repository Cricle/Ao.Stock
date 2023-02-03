using System.Collections.Generic;

namespace Ao.Stock.IntangibleProviders
{
    public class MySqlSQLIntangibleProvider : SQLIntangibleProvider
    {
        public const string SslModeKey = "mysql.SslMode";

        public static readonly MySqlSQLIntangibleProvider Default = new MySqlSQLIntangibleProvider();

        private static readonly HashSet<string> keys = GetKnowKeysWith(new[] { SslModeKey });

        public override IEnumerable<string> GetKeys()
        {
            return keys;
        }
        public override bool TryInverseReplace(string key, out string? result)
        {
            result = key;
            switch (key)
            {
                case "Server":
                    result = HostKey;
                    break;
                case "Port":
                    result = PortKey;
                    break;
                case "Pwd":
                    result = PasswordKey;
                    break;
                case "UId":
                    result = UserNameKey;
                    break;
                case "Database":
                    result = DatabaseKey;
                    break;
                case "SslMode":
                    result = SslModeKey;
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
                    result = "Server";
                    break;
                case PortKey:
                    result = "Port";
                    break;
                case UserNameKey:
                    result = "Uid";
                    break;
                case PasswordKey:
                    result = "Pwd";
                    break;
                case DatabaseKey:
                    result = "Database";
                    break;
                case SslModeKey:
                    result = "SslMode";
                    break;
                case ConnectTimeoutKey:
                    result = "Connect Timeout";
                    break;
                default:
                    return false;

            }
            return true;
        }
    }

}
