using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MyLab.ExpressionTools
{
    class MemberExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.MemberAccess;
        }

        public object GetValue(Expression expression)
        {
            var ma = (MemberExpression)expression;

            var targetObject = ma.Expression?.GetValue<object>();

            switch (ma.Member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)ma.Member).GetValue(targetObject);
                case MemberTypes.Method:
                    return ((PropertyInfo)ma.Member).GetValue(targetObject);
                case MemberTypes.Property:
                    return ((PropertyInfo)ma.Member).GetValue(targetObject);
                default:
                    throw new NotSupportedException($"Member access '{ma.Member.MemberType}' not supported");
            }
        }
    }
}