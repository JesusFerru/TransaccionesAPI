using Ardalis.Specification;

namespace Transacciones.Core.SharedKernel.Interfaces
{
    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
    {
        Task<List<T>> ListAsyncAsNoTrackin(CancellationToken cancellationToken = default);
    }
}
