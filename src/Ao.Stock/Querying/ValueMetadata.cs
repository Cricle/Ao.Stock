using System;
using System.Text;

namespace Ao.Stock.Querying
{
    public class ValueMetadata<T> : QueryMetadata, IEquatable<ValueMetadata<T>>, IQueryMetadata
    {
        public ValueMetadata(T value, bool quto)
        {
            Value = value;
            Quto = quto;
        }

        public ValueMetadata(T value)
            : this(value, false)
        {
        }

        public T Value { get; }

        public bool Quto { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Quto);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ValueMetadata<T>);
        }

        public virtual bool Equals(ValueMetadata<T>? other)
        {
            if (other == null)
            {
                return false;
            }
            if (Value == null && other.Value == null)
            {
                return true;
            }
            if (Value == null || other.Value == null)
            {
                return false;
            }
            return Value.Equals(other.Value) && Quto == other.Quto;
        }

        protected virtual string QutoString()
        {
            return "\"" + ToString(Value) + "\"";
        }

        protected virtual string ToString(T value)
        {
            return Value?.ToString() ?? "null";
        }

        public override string ToString()
        {
            if (Quto)
            {
                return QutoString();
            }
            return ToString(Value);
        }

    }
}
