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
            get => this.GetOrDefault<string>(DatabaseKey);
            set => this[DatabaseKey] = value;
        }

        public string Host
        {
            get => this.GetOrDefault<string>(HostKey);
            set => this[HostKey] = value;
        }
        public short Port
        {
            get => this.GetOrDefault<short>(PortKey);
            set => this[PortKey] = value;
        }
        public string UserName
        {
            get => this.GetOrDefault<string>(UserNameKey);
            set => this[UserNameKey] = value;
        }
        public string Password
        {
            get => this.GetOrDefault<string>(PasswordKey);
            set => this[PasswordKey] = value;
        }
        public int ConnectTimeout
        {
            get => this.GetOrDefault<int>(ConnectTimeoutKey);
            set => this[ConnectTimeoutKey] = value;
        }

        public override string ToString()
        {
            return string.Join(";", this.Select(x => $"{x.Key}={x.Value}"));
        }

        public virtual string ReplaceKey(string key)
        {
            return key;
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

        public string NoContainsIgnore(string key, string connect = "=", string end = ";")
        {
            if (TryGetValue(key, out var obj))
            {
                return $"{ReplaceKey(key)}{connect}{obj}{end}";
            }
            return string.Empty;
        }

        protected virtual bool IsOthers(KeyValuePair<object, object> input)
        {
            return false;
        }

        public string JoinOthers()
        {
            return string.Join(";", this.Where(x => !KnowsSet.Contains(x.Key) && !IsOthers(x)).Select(x => $"{x.Key}={x.Value}"));
        }
    }

}
