using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Transacciones.Core.SharedKernel.Interfaces;

namespace Transacciones.Infrastructure.Data
{
    public class EfRepository<T> : RepositoryBase<T>, IRepositoryBase<T>, IReadRepositoryBase<T>, IRepository<T>
        where T : class, IAggregateRoot
    {
        public EfRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

    }
}
