using Ardalis.Specification;

namespace Transacciones.Core.SharedKernel.Interfaces
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
    {

    }
}
