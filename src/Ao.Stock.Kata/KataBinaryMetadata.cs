using Ao.Stock.Querying;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ao.Stock.Kata
{
    public class KataBinaryMetadata<TLeft, TRight> : BinaryMetadata<TLeft, TRight>
    {
        public KataBinaryMetadata(TLeft left, ExpressionType expressionType, TRight right) : base(left, expressionType, right)
        {
        }
        protected override string Middle(string op)
        {
            if (op=="==")
            {
                op = "=";
            }
            return base.Middle(op);
        }
    }
}
