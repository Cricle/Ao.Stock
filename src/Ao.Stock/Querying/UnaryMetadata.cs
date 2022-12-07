using System;
using System.Linq.Expressions;

namespace Ao.Stock.Querying
{
    public class UnaryMetadata : QueryMetadata, IEquatable<UnaryMetadata>, IQueryMetadata, IUnaryMetadata
    {
        public UnaryMetadata(object left, ExpressionType expressionType)
        {
            Left = new ValueMetadata(left);
            ExpressionType = expressionType;
        }
        public UnaryMetadata(IQueryMetadata left, ExpressionType expressionType)
        {
            Left = left;
            ExpressionType = expressionType;
        }

        public IQueryMetadata Left { get; }

        public ExpressionType ExpressionType { get; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UnaryMetadata);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, ExpressionType);
        }
        public virtual string? LeftToString()
        {
            return Left?.ToString() ?? "null";
        }
        protected virtual string PreCombine(string op)
        {
            return string.Concat(op, " ", LeftToString());
        }
        protected virtual string PostCombine(string op)
        {
            return string.Concat(LeftToString(), " ", op);
        }

        public override string ToString()
        {
            switch (ExpressionType)
            {
                case ExpressionType.UnaryPlus:
                    return PreCombine("+");
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    return PreCombine("-");
                case ExpressionType.Decrement:
                    return PostCombine("-1");
                case ExpressionType.Increment:
                    return PostCombine("+1");
                case ExpressionType.PreIncrementAssign:
                    return PreCombine("++");
                case ExpressionType.PreDecrementAssign:
                    return PreCombine("--");
                case ExpressionType.PostIncrementAssign:
                    return PostCombine("++");
                case ExpressionType.PostDecrementAssign:
                    return PostCombine("--");
                case ExpressionType.OnesComplement:
                    return PreCombine("~");
                case ExpressionType.Not:
                    return PreCombine("!");
                case ExpressionType.IsTrue:
                    return PostCombine("is true");
                case ExpressionType.IsFalse:
                    return PostCombine("is false");
                default:
                    throw new NotSupportedException(ExpressionType.ToString());
            }
        }

        public bool Equals(UnaryMetadata? other)
        {
            if (other == null)
            {
                return false;
            }
            return CheckLeftEquals(other.Left) && other.ExpressionType == ExpressionType;
        }

        protected virtual bool CheckLeftEquals(in IQueryMetadata otherLeft)
        {
            if (otherLeft == null && Left == null)
            {
                return true;
            }
            if (otherLeft == null || Left == null)
            {
                return false;
            }
            return otherLeft.Equals(Left);
        }
    }
}
