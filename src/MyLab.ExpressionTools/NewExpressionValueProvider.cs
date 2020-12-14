using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MyLab.ExpressionTools
{
    class NewExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.New;
        }

        public object GetValue(Expression expression)
        {
            var ne = (NewExpression)expression;
            var ctorParams = ne.Constructor.GetParameters();

            var args = new List<object>();
            for (int i = 0; i < ne.Arguments.Count; i++)
            {
                var a = ne.Arguments[i];
                var ctorA = ctorParams[i];

                if (ctorA.ParameterType.BaseType == typeof(MulticastDelegate) &&
                    (a is LambdaExpression lambda))
                {
                    args.Add(lambda.Compile());
                }
                else
                {
                    args.Add(a.GetValue<object>());
                }

            }

            return ne.Constructor.Invoke(args.ToArray());
        }
    }
}