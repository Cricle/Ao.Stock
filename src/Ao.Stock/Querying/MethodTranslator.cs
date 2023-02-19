using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Ao.Stock.Querying
{
    public class MethodTranslator<T> : Dictionary<string, string>, IMethodTranslator<T>
    {

        public MethodTranslator()
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        public MethodTranslator(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
        {
        }

        public MethodTranslator(IEqualityComparer<string> comparer) : base(comparer)
        {
        }

        public MethodTranslator(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
        {
        }

        public MethodTranslator(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        public MethodTranslator(int capacity) : base(capacity)
        {
        }

        protected MethodTranslator(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string Translate(IMethodMetadata metadata, T context)
        {
            if (TryGetValue(metadata.Method, out var format))
            {
                return OnTranslate(metadata, context, format);
            }
            throw new NotSupportedException(metadata.Method);
        }

        protected virtual string?[] ToFormatArray(IMethodMetadata metadata, T context)
        {
            var args = new string?[metadata.Args == null ? 1 : metadata.Args.Count + 1];
            args[0] = metadata.Method;
            if (metadata.Args != null)
            {
                for (int i = 0; i < metadata.Args.Count; i++)
                {
                    args[i + 1] = ToString(metadata.Args[i], context);
                }
            }
            return args;
        }

        protected virtual string OnTranslate(IMethodMetadata metadata, T context, string formatter)
        {
            var args = ToFormatArray(metadata, context);
            if (formatter.Contains(KnowsMethods.AllPlaceholder))
            {
                formatter = formatter.Replace(KnowsMethods.AllPlaceholder, string.Join(",", args.Skip(1)));
            }
            if (formatter.Contains(KnowsMethods.RangeSkip1))
            {
                formatter = formatter.Replace(KnowsMethods.RangeSkip1, string.Join(",", args.Skip(2)));
            }
            if (formatter.Contains(KnowsMethods.AllOrAlso))
            {
                formatter = formatter.Replace(KnowsMethods.AllOrAlso, string.Join("||", args.Skip(1)));
            }
            return string.Format(formatter, args);
        }

        public virtual string? ToString(IQueryMetadata metadata, T context)
        {
            if (metadata is MethodMetadata method)
            {
                return Translate(method, context);
            }
            return metadata.ToString();
        }

        string IMethodTranslator.Translate(IMethodMetadata metadata, object context)
        {
            return Translate(metadata, (T)context);
        }
    }

}
