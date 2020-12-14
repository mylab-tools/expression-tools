using System.Linq.Expressions;

namespace MyLab.ExpressionTools
{
    interface IExpressionValueProvider
    {
        bool Predicate(Expression expression);

        object GetValue(Expression expression);
    }
}
