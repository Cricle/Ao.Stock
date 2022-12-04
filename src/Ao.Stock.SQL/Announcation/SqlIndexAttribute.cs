using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Stock.SQL.Announcation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SqlIndexAttribute:Attribute
    {
    }
}
