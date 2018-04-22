
namespace RM.Resources.Specification
{
    using System;
    using System.Linq.Expressions;

    internal class GenericSpecification<T> : Specification<T>
    {
        readonly Expression<Func<T, bool>> _expression;

        internal static readonly Specification<T> All = new GenericSpecification<T>(x => true);

        internal static readonly Specification<T> None = new GenericSpecification<T>(x => false);

        Func<T, bool> _compiledFunc;

        public GenericSpecification(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
        }

        public override Expression<Func<T, bool>> Expression
        {
            get { return _expression; }
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            _compiledFunc = _compiledFunc ?? Expression.Compile();
            return _compiledFunc(candidate);
        }

    }
}
