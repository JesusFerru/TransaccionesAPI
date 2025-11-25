using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Transacciones.Core.SharedKernel.Interfaces;

namespace Transacciones.Infrastructure.Data
{
    public class EfRepository<T> : IRepository<T> where T : class, IAggregateRoot
    {
        private readonly AppDbContext _dbContext;
        private readonly ISpecificationEvaluator _specEvaluator = SpecificationEvaluator.Default;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // WRITE OPERATIONS (NO SaveChanges)
        public Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        // READ OPERATIONS
        public async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
        {
            return await _dbContext.Set<T>().FindAsync(new object?[] { id }, cancellationToken);
        }

        public async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpec(specification).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpec(specification).ToListAsync(cancellationToken);
        }

        public async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpec(specification, true).AnyAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().AnyAsync(cancellationToken);
        }

        public async Task<List<T>> ListAsyncAsNoTrackin(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        // SPECIFICATION SUPPORT
        private IQueryable<T> ApplySpec(ISpecification<T> spec, bool criteriaOnly = false)
        {
            return _specEvaluator.GetQuery(
                _dbContext.Set<T>().AsQueryable(),
                spec,
                criteriaOnly
            );
        }
    }
}
