using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MyLab.ExpressionTools
{
    class MemberInitExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.MemberInit;
        }

        public object GetValue(Expression expression)
        {
            var mi = (MemberInitExpression)expression;

            var obj = mi.NewExpression.GetValue<object>();
            foreach (var memberBinding in mi.Bindings)
            {
                switch (memberBinding.BindingType)
                {
                    case MemberBindingType.Assignment:
                    {
                        var ma = (MemberAssignment)memberBinding;
                        var memberValue = ma.Expression.GetValue<object>();

                        switch (memberBinding.Member.MemberType)
                        {
                            case MemberTypes.Field:
                                ((FieldInfo)memberBinding.Member).SetValue(obj, memberValue);
                                break;
                            case MemberTypes.Property:
                                ((PropertyInfo)memberBinding.Member).SetValue(obj, memberValue);
                                break;
                            default:
                                throw new NotSupportedException($"Member type '{memberBinding.Member.MemberType}' not supported");
                        }
                    }
                        break;
                    //case MemberBindingType.MemberBinding:
                    //    break;
                    case MemberBindingType.ListBinding:
                    {
                        var lb = (MemberListBinding)memberBinding;

                        switch (lb.Member.MemberType)
                        {
                            case MemberTypes.Property:
                            {
                                var propMember = (PropertyInfo)lb.Member;
                                var propertyVal = propMember.GetValue(obj);
                                foreach (var lbInitializer in lb.Initializers)
                                {
                                    lbInitializer.AddMethod.Invoke(propertyVal,
                                        lbInitializer.Arguments
                                            .Select(a => ExpressionExtensions.GetValue<object>(a))
                                            .ToArray());
                                }
                            }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return obj;
        }
    }
}