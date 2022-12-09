using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Ao.Stock.Querying
{
    public class ExpressionParser
    {
        public static readonly ExpressionParser Default = new ExpressionParser();

        protected virtual string GetMemberName(MemberExpression member)
        {
            return member.Member.Name;
        }
        protected virtual string GetMethodName(MethodCallExpression methodCall)
        {
            return methodCall.Method.Name;
        }

        public virtual IQueryMetadata Parse(Expression expression)
        {
            if (expression is ConstantExpression constant)
            {
                return new ValueMetadata(constant.Value);
            }
            else if (expression is LambdaExpression lambda)
            {
                return Parse(lambda.Body);
            }
            else if (expression is MemberExpression member)
            {
                if (member.Member is PropertyInfo|| member.Member is FieldInfo)
                {
                    return new ValueMetadata(GetMemberName(member), true);
                }
                return Parse(member.Expression);
            }
            else if (expression is MethodCallExpression methodCall)
            {
                var pars = new IQueryMetadata[methodCall.Arguments.Count];
                for (int i = 0; i < methodCall.Arguments.Count; i++)
                {
                    pars[i] = Parse(methodCall.Arguments[i]);
                }
                return new MethodMetadata(GetMethodName(methodCall), pars);
            }
            else if (expression is BinaryExpression binary)
            {
                var left = Parse(binary.Left);
                var right = Parse(binary.Right);
                return new BinaryMetadata(left, binary.NodeType, right);
            }
            else if (expression is UnaryExpression unary)
            {
                var left = Parse(unary.Operand);
                return new UnaryMetadata(left, unary.NodeType);
            }
            else if (expression is BlockExpression block)
            {
                var metadatas = new MultipleQueryMetadata(block.Expressions.Count);
                for (int i = 0; i < block.Expressions.Count; i++)
                {
                    metadatas.Add(Parse(block.Expressions[i]));
                }
                return metadatas;
            }
            else
            {
                throw new NotSupportedException(expression.ToString());
            }
        }
    }
}
