using System;
using System.Collections.Generic;
using System.Linq;

namespace Ao.Stock
{
    public class SQLIntangibleContext : IntangibleContext
    {
        private static readonly HashSet<string> KnowsSet = new HashSet<string>
        {
            HostKey,PortKey,UserNameKey,PasswordKey,ConnectTimeoutKey,DatabaseKey
        };

        public const string HostKey = "conn.Host";

        public const string PortKey = "conn.Port";

        public const string UserNameKey = "conn.UserName";

        public const string PasswordKey = "conn.Password";

        public const string ConnectTimeoutKey = "conn.ConnectTimeout";

        public const string DatabaseKey = "conn.DefaultDatabase";

        public string Database
        {
            get => GetOrDefault<string>(DatabaseKey);
            set => this[DatabaseKey] = value;
        }

        public string Host
        {
            get => GetOrDefault<string>(HostKey);
            set => this[HostKey] = value;
        }
        public short Port
        {
            get => GetOrDefault<short>(PortKey);
            set => this[PortKey] = value;
        }
        public string UserName
        {
            get => GetOrDefault<string>(UserNameKey);
            set => this[UserNameKey] = value;
        }
        public string Password
        {
            get => GetOrDefault<string>(PasswordKey);
            set => this[PasswordKey] = value;
        }
        public int ConnectTimeout
        {
            get => GetOrDefault<int>(ConnectTimeoutKey);
            set => this[ConnectTimeoutKey] = value;
        }

        public override string ToString()
        {
            return string.Join(";", this.Select(x => $"{x.Key}={x.Value}"));
        }

        public override int GetHashCode()
        {
            var hc = new HashCode();
            foreach (var item in this)
            {
                hc.Add(item.Key);
                hc.Add(item.Value);
            }
            return hc.ToHashCode();
        }

        public string NoContainsIgnore(string key,string actualName, string connect = "=",string end=";")
        {
            if (TryGetValue(key,out var obj))
            {
                return $"{actualName}{connect}{obj}{end}";
            }
            return string.Empty;
        }

        protected virtual bool IsNotOthers(KeyValuePair<object,object> input)
        {
            return false;
        }

        public string JoinOthers()
        {
            return string.Join(";", this.Where(x => !KnowsSet.Contains(x.Key)&& !IsNotOthers(x)).Select(x => $"{x.Key}={x.Value}"));
        }
    }

}
