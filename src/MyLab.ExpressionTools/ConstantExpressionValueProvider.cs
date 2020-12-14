using System.Linq.Expressions;

namespace MyLab.ExpressionTools
{
    class ConstantExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression == null || expression.NodeType == ExpressionType.Constant;
        }

        public object GetValue(Expression expression)
        {
            return ((ConstantExpression)expression)?.Value;
        }
    }
}