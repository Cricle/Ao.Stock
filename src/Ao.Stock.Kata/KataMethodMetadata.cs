using Ao.Stock.Querying;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Stock.Kata
{
    public class KataMethodMetadata : MethodMetadata
    {
        public KataMethodMetadata(string method) : base(method)
        {
        }

        public KataMethodMetadata(string method, IList<IQueryMetadata> args) : base(method, args)
        {
        }

        public Compiler Compiler { get; set; }

        public override string ToString()
        {
            if (IsMethodIgnoreCase("Contains") ||
                IsMethodIgnoreCase("Like"))
            {
                return Compiler.Wrap(Args[0].ToString()) + " like '" + (Args[1].ToString()) + "'";
            }
            return base.ToString();
        }
    }
}
