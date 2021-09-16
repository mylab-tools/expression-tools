using System;
using System.Linq;
using System.Linq.Expressions;

namespace MyLab.ExpressionTools
{
    /// <summary>
    /// Extension methods for <see cref="Expression"/>
    /// </summary>
    public static class ExpressionExtensions
    {
        private static readonly IExpressionValueProvider[] ValueProviders =
        {
            new ConstantExpressionValueProvider(),
            new MemberExpressionValueProvider(),
            new CallExpressionValueProvider(),
            new NewExpressionValueProvider(),
            new MemberInitExpressionValueProvider(),
            new ConvertExpressionValueProvider(),
            new LambdaExpressionValueProvider(),
            new ListInitExpressionValueProvider()
        };

        /// <summary>
        /// Gets expression value
        /// </summary>
        public static T GetValue<T>(this Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var valProvider = ValueProviders.FirstOrDefault(p => p.Predicate(expression));

            if (valProvider == null)
                throw new NotSupportedException($"Expression type '{expression.NodeType}' not supported");

            return (T) valProvider.GetValue(expression);
        }
    }
}