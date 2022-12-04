using System;
using System.Reflection;

namespace Ao.Stock
{
    public class ReflectionOptions
    {
        public Func<PropertyInfo?, string>? PropertyNameGetter { get; set; }

        public Func<Type?, string>? TypeNameGetter { get; set; }

        public bool AttributeInherit { get; set; }
    }
}
