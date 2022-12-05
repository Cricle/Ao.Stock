using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Ao.Stock.Querying
{
    public class MethodMetadata : QueryMetadata,IEquatable<MethodMetadata>, IQueryMetadata, IExpressionTypeProvider
    {
        public MethodMetadata(string method, IList<IQueryMetadata>? args)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));
            Args = args;
        }

        public MethodMetadata(string method)
            :this(method, null)
        {
        }

        public string Method { get; }

        public ExpressionType ExpressionType { get; } = ExpressionType.Call;

        public IList<IQueryMetadata>? Args { get; }

        public override int GetHashCode()
        {
            var hc = new HashCode();
            hc.Add(Method);
            hc.Add(ExpressionType);
            if (Args != null)
            {
                for (int i = 0; i < Args.Count; i++)
                {
                    hc.Add(Args[i]);
                }
            }
            return hc.ToHashCode();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MethodMetadata);
        }
        protected virtual string ToString(IQueryMetadata arg)
        {
            return arg?.ToString() ?? "null";
        }
        public override string ToString()
        {
            return $"{Method}({(Args == null ? string.Empty : string.Join(",", Args.Select(x => ToString(x))))})";
        }

        public bool Equals(MethodMetadata? other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Method == Method &&
                ArgsEquals(other.Args);
        }

        protected virtual bool ArgsEquals(IList<IQueryMetadata>? args)
        {
            if (args == null && Args == null)
            {
                return true;
            }
            if (args == null || Args == null)
            {
                return false;
            }
            return args.Count == Args.Count &&
                args.SequenceEqual(Args);
        }

    }
}
