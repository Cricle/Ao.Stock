﻿using System;
using System.Runtime.CompilerServices;

namespace Ao.Stock.Querying
{
    public class DefaultMethodWrapper : IMethodWrapper
    {
        public static readonly DefaultMethodWrapper MySql = new DefaultMethodWrapper("`", "`", "'", "'");
        public static readonly DefaultMethodWrapper SqlServer = new DefaultMethodWrapper("[", "]", "'", "'");
        public static readonly DefaultMethodWrapper MariaDB = MySql;
        public static readonly DefaultMethodWrapper Sqlite = new DefaultMethodWrapper("`", "`", "'", "'");
        public static readonly DefaultMethodWrapper Oracle = new DefaultMethodWrapper("\"", "\"", "'", "'");
        public static readonly DefaultMethodWrapper Postgres = new DefaultMethodWrapper("\"", "\"", "'", "'");

        public DefaultMethodWrapper(string qutoStart, string qutoEnd, string valueStart, string valueEnd)
        {
            QutoStart = qutoStart;
            QutoEnd = qutoEnd;
            ValueStart = valueStart;
            ValueEnd = valueEnd;
        }

        public string QutoStart { get; }

        public string QutoEnd { get; }

        public string ValueStart { get; }

        public string ValueEnd { get; }

        public virtual string Quto<T>(T input)
        {
            return QutoStart + input + QutoEnd;
        }

        public virtual string WrapValue<T>(T input)
        {
            if (input == null)
            {
                return "null";
            }
            if (input is string)
            {
                return ValueStart + input + ValueEnd;
            }
            else if (input is DateTime || input is DateTime?)
            {                
                var dt = Unsafe.As<T, DateTime>(ref input);
                return ValueStart + dt.ToString("yyyy-MM-dd HH:mm:ss.ffff") + ValueEnd;
            }
            return input.ToString();
        }
    }

}
