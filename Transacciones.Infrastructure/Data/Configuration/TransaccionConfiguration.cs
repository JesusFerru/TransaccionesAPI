using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transacciones.Core.Entities.TransaccionAggregate;

namespace Transacciones.Infrastructure.Data.Configuration
{
    public class TransaccionConfiguration : IEntityTypeConfiguration<Transaccion>
    {
        public void Configure(EntityTypeBuilder<Transaccion> builder)
        {
            builder.ToTable(nameof(Transaccion));

            builder.HasKey(t => t.Id);

        }
    }
}
