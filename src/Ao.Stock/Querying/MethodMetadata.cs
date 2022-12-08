using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Ao.Stock.Querying
{
    public class MethodMetadata : QueryMetadata,IEquatable<MethodMetadata>, IMethodMetadata
    {
        public static MethodMetadata Values(string method, bool quto, params object[]? args)
        {
            return new MethodMetadata(method, args?.Select(x => new ValueMetadata(x, quto)).ToArray());
        }
        public static MethodMetadata Values(string method, params object[]? args)
        {
            return Values(method, false, args);
        }
        public static MethodMetadata Quto(string method, params object[]? args)
        {
            return Values(method, true, args);
        }

        public MethodMetadata(string method,params IQueryMetadata[]? args)
            :this(method,null,args)
        {
        }

        public MethodMetadata(string method)
            : this(method, (MethodInfo?)null, (IList<IQueryMetadata>?)null)
        {
        }
        public MethodMetadata(MethodInfo method)
            : this(method.Name, method, (IList<IQueryMetadata>?)null)
        {
        }
        public MethodMetadata(MethodInfo method, IList<IQueryMetadata>? args)
            : this(method.Name, method, args)
        {
        }
        public MethodMetadata(string method,MethodInfo? info, IList<IQueryMetadata>? args)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));
            MethodInfo = info;
            Args = args;
        }
        public string Method { get; }

        public MethodInfo? MethodInfo { get; }

        public ExpressionType ExpressionType { get; } = ExpressionType.Call;

        public IList<IQueryMetadata>? Args { get; }

        public override IEnumerable<IQueryMetadata> GetChildren()
        {
            if (Args!=null)
            {
                return Args;
            }
            return Enumerable.Empty<IQueryMetadata>();  
        }
        public bool IsMethodIgnoreCase(string method)
        {
            return string.Equals(method, Method, StringComparison.OrdinalIgnoreCase);
        }
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
