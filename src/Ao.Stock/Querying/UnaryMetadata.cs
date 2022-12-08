using System;
using System.Collections.Generic;
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

        public override IEnumerable<IQueryMetadata> GetChildren()
        {
            yield return Left;
        }

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

        public virtual string GetToken()
        {
            switch (ExpressionType)
            {
                case ExpressionType.UnaryPlus:
                    return "+";
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    return "-";
                case ExpressionType.Decrement:
                    return "-1";
                case ExpressionType.Increment:
                    return "+1";
                case ExpressionType.PreIncrementAssign:
                    return "++";
                case ExpressionType.PreDecrementAssign:
                    return "--";
                case ExpressionType.PostIncrementAssign:
                    return "++";
                case ExpressionType.PostDecrementAssign:
                    return "--";
                case ExpressionType.OnesComplement:
                    return "~";
                case ExpressionType.Not:
                    return "!";
                case ExpressionType.IsTrue:
                    return "is true";
                case ExpressionType.IsFalse:
                    return "is false";
                default:
                    throw new NotSupportedException(ExpressionType.ToString());
            }

        }
        public bool IsPostCombine()
        {
            return !IsPreCombine();
        }
        public bool IsPreCombine()
        {
            switch (ExpressionType)
            {
                case ExpressionType.UnaryPlus:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.OnesComplement:
                case ExpressionType.Not:
                    return true;
                default:
                    return false;
            }
        }
        public override string ToString()
        {
            var token = GetToken();
            if (IsPreCombine())
            {
                return PreCombine(token);
            }
            return PostCombine(token);
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
