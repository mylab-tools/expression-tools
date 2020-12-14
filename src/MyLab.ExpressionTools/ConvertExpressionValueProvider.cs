using System.Linq.Expressions;

namespace MyLab.ExpressionTools
{
    internal class ConvertExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.Quote;
        }

        public object GetValue(Expression expression)
        {
            var uExpr = (UnaryExpression)expression;
            return uExpr.Operand.GetValue<object>();
        }
    }
}