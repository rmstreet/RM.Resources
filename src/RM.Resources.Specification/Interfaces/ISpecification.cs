
namespace RM.Resources.Specification.Interfaces
{
    using System;
    using System.Linq.Expressions;

    public interface ISpecification
    {
    }

    public interface ISpecification<T> : ISpecification
    {
        bool IsSatisfiedBy(T candidate);

        Expression<Func<T, bool>> Expression { get; }
    }
}
