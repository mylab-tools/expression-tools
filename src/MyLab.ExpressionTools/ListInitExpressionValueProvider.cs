using System.Linq;
using System.Linq.Expressions;

namespace MyLab.ExpressionTools
{
    internal class ListInitExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.ListInit;
        }

        public object GetValue(Expression expression)
        {
            var liExpr = (ListInitExpression) expression;

            var target = liExpr.NewExpression.GetValue<object>();

            foreach (var initializer in liExpr.Initializers)
            {
                var addAgrs = initializer.Arguments.Select(a => a.GetValue<object>()).ToArray();

                initializer.AddMethod.Invoke(target, addAgrs);
            }

            return target;
        }
    }
}