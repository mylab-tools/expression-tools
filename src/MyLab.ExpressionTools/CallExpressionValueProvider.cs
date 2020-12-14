using System;
using System.Linq;
using System.Linq.Expressions;

namespace MyLab.ExpressionTools
{
    class CallExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.Call;
        }

        public object GetValue(Expression expression)
        {
            var call = ((MethodCallExpression)expression);

            var targetObject = call.Object != null
                ? call.Object.GetValue<object>()
                : null;

            var parameters = call.Method.GetParameters();

            var args = call.Arguments
                .Select((expr, i) =>
                {
                    var paramIType = parameters[i].ParameterType;

                    if (typeof(Expression).IsAssignableFrom(paramIType))
                    {
                        switch (expr.NodeType)
                        {
                            case ExpressionType.Lambda:
                                return expr;
                            case ExpressionType.Quote:
                                return ((UnaryExpression)expr).Operand;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    return ExpressionExtensions.GetValue<object>(expr);
                })
                .ToArray();

            return call.Method.Invoke(targetObject, args);
        }
    }
}