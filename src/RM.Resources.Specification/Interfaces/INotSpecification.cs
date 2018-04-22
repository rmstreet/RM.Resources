
namespace RM.Resources.Specification.Interfaces
{
    public interface INotSpecification<T> : ISpecification<T>
    {
        ISpecification<T> Inner { get; }
    }
}
