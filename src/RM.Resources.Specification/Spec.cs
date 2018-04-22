
namespace RM.Resources.Specification
{
    using Interfaces;
    using System;
    using System.Linq.Expressions;

    public static class Spec
    {
        public static ISpecification<T> For<T>(Expression<Func<T, bool>> expression)
        {
            return new GenericSpecification<T>(expression);
        }

        public static Specification<T> All<T>()
        {
            return GenericSpecification<T>.All;
        }

        public static Specification<T> None<T>()
        {
            return GenericSpecification<T>.None;
        }
    }
}
