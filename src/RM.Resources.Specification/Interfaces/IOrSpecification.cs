
namespace RM.Resources.Specification.Interfaces
{
    public interface IOrSpecification<T> : ISpecification<T>
    {
        ISpecification<T> Spec1 { get; }

        ISpecification<T> Spec2 { get; }
    }
}
