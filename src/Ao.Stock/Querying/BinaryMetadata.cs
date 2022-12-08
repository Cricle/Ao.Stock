using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Ao.Stock.Querying
{
    public class BinaryMetadata : QueryMetadata,IEquatable<BinaryMetadata>, IBinaryMetadata
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

        public override IEnumerable<IQueryMetadata> GetChildren()
        {
            yield return Left;
            yield return Right;
        }

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

        public virtual string GetToken()
        {
            switch (ExpressionType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.And:
                    return "&";
                case ExpressionType.AndAlso:
                    return "&&";
                case ExpressionType.Coalesce:
                    return "??";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Equal:
                    return "==";
                case ExpressionType.ExclusiveOr:
                    return "^";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LeftShift:
                    return "<<";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                case ExpressionType.NotEqual:
                    return "!=";
                case ExpressionType.Or:
                    return "|";
                case ExpressionType.OrElse:
                    return "||";
                case ExpressionType.RightShift:
                    return ">>";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.TypeAs:
                    return "as";
                case ExpressionType.TypeIs:
                    return "is";
                case ExpressionType.Assign:
                    return "=";
                case ExpressionType.AddAssign:
                    return "+=";
                case ExpressionType.AndAssign:
                    return "&=";
                case ExpressionType.DivideAssign:
                    return "/=";
                case ExpressionType.ExclusiveOrAssign:
                    return "^=";
                case ExpressionType.LeftShiftAssign:
                    return "<<=";
                case ExpressionType.ModuloAssign:
                    return "%=";
                case ExpressionType.MultiplyAssign:
                    return "*=";
                case ExpressionType.OrAssign:
                    return "|=";
                case ExpressionType.PowerAssign:
                    return "^=";
                case ExpressionType.RightShiftAssign:
                    return ">>=";
                case ExpressionType.SubtractAssign:
                case ExpressionType.AddAssignChecked:
                    return "-=";
                case ExpressionType.MultiplyAssignChecked:
                    return "*=";
                case ExpressionType.SubtractAssignChecked:
                    return "-=";
                default:
                    throw new NotSupportedException(ExpressionType.ToString());
            }
        }

        public override string ToString()
        {
            if (ExpressionType== ExpressionType.ArrayIndex)
            {
                return ArrayIndex();
            }
            var token = GetToken();
            return Middle(token);
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
    }
}
