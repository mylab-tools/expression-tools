using System.Linq.Expressions;

namespace MyLab.ExpressionTools
{
    internal class LambdaExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.Lambda;
        }

        public object GetValue(Expression expression)
        {
            var lambda = (LambdaExpression)expression;
            return lambda.Body.GetValue<object>();
        }
    }
}