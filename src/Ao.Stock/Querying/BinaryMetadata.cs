using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Ao.Stock.Querying
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class BinaryMetadata : IEquatable<BinaryMetadata>, IQueryMetadata, IBinaryMetadata
    {
        public BinaryMetadata(object left, ExpressionType expressionType, object right)
        {
            Left = new ValueMetadata(left);
            ExpressionType = expressionType;
            Right = new ValueMetadata(right);
        }
        public BinaryMetadata(IQueryMetadata left, ExpressionType expressionType, IQueryMetadata right)
        {
            Left = left;
            ExpressionType = expressionType;
            Right = right;
        }

        public IQueryMetadata Left { get; }

        public ExpressionType ExpressionType { get; }

        public IQueryMetadata Right { get; }

        protected virtual string Middle(string op)
        {
            return string.Concat(LeftToString(), " ", op, " ", RightToString());
        }
        protected virtual string ArrayIndex()
        {
            return string.Concat(LeftToString(), "[", RightToString(), "]");
        }

        public virtual string? LeftToString()
        {
            return Left?.ToString() ?? "null";
        }
        public virtual string? RightToString()
        {
            return Right?.ToString() ?? "null";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, ExpressionType, Right);
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as BinaryMetadata);
        }
        public override string ToString()
        {
            switch (ExpressionType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return Middle("+");
                case ExpressionType.And:
                    return Middle("&");
                case ExpressionType.AndAlso:
                    return Middle("&&");
                case ExpressionType.Coalesce:
                    return Middle("??");
                case ExpressionType.Divide:
                    return Middle("/");
                case ExpressionType.Equal:
                    return Middle("==");
                case ExpressionType.ExclusiveOr:
                    return Middle("^");
                case ExpressionType.GreaterThan:
                    return Middle(">");
                case ExpressionType.GreaterThanOrEqual:
                    return Middle(">=");
                case ExpressionType.LeftShift:
                    return Middle("<<");
                case ExpressionType.LessThan:
                    return Middle("<");
                case ExpressionType.LessThanOrEqual:
                    return Middle("<=");
                case ExpressionType.Modulo:
                    return Middle("%");
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return Middle("*");
                case ExpressionType.NotEqual:
                    return Middle("!=");
                case ExpressionType.Or:
                    return Middle("|");
                case ExpressionType.OrElse:
                    return Middle("||");
                case ExpressionType.RightShift:
                    return Middle(">>");
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return Middle("-");
                case ExpressionType.TypeAs:
                    return Middle("as");
                case ExpressionType.TypeIs:
                    return Middle("is");
                case ExpressionType.Assign:
                    return Middle("=");
                case ExpressionType.AddAssign:
                    return Middle("+=");
                case ExpressionType.AndAssign:
                    return Middle("&=");
                case ExpressionType.DivideAssign:
                    return Middle("/=");
                case ExpressionType.ExclusiveOrAssign:
                    return Middle("^=");
                case ExpressionType.LeftShiftAssign:
                    return Middle("<<=");
                case ExpressionType.ModuloAssign:
                    return Middle("%=");
                case ExpressionType.MultiplyAssign:
                    return Middle("*=");
                case ExpressionType.OrAssign:
                    return Middle("|=");
                case ExpressionType.PowerAssign:
                    return Middle("^=");
                case ExpressionType.RightShiftAssign:
                    return Middle(">>=");
                case ExpressionType.SubtractAssign:
                case ExpressionType.AddAssignChecked:
                    return Middle("-=");
                case ExpressionType.MultiplyAssignChecked:
                    return Middle("*=");
                case ExpressionType.SubtractAssignChecked:
                    return Middle("-=");
                case ExpressionType.ArrayIndex:
                    return ArrayIndex();
                default:
                    throw new NotSupportedException(ExpressionType.ToString());
            }
        }

        public bool Equals(BinaryMetadata? other)
        {
            if (other == null)
            {
                return false;
            }
            return CheckLeftEquals(other.Left) && CheckRightEquals(other.Right) && other.ExpressionType == ExpressionType;
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
        protected virtual bool CheckRightEquals(in IQueryMetadata otherRight)
        {
            if (otherRight == null && Right == null)
            {
                return true;
            }
            if (otherRight == null || Right == null)
            {
                return false;
            }
            return otherRight.Equals(Right);
        }


        public void ToString(StringBuilder builder)
        {
            builder.Append(ToString());
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
