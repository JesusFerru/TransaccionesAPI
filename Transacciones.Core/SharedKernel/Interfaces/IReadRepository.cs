using Ardalis.Specification;

namespace Transacciones.Core.SharedKernel.Interfaces
{
    public interface IReadRepository<T> where T : class, IAggregateRoot
    {
        Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull;

        Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task<List<T>> ListAsync(CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(CancellationToken cancellationToken = default);

        Task<List<T>> ListAsyncAsNoTrackin(CancellationToken cancellationToken = default);
    }
}
