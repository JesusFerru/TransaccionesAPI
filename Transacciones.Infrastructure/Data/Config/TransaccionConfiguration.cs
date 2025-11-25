using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transacciones.Core.Entities.TransaccionAggregate;

namespace Transacciones.Infrastructure.Data.Config
{
    public class TransaccionConfiguration : IEntityTypeConfiguration<Transaccion>
    {
        public void Configure(EntityTypeBuilder<Transaccion> builder)
        {
            builder.Property(t => t.TipoTransaccion)
                .HasConversion(
                    v => v.ToString(),
                    v => (TipoTransaccionEnum)Enum.Parse(typeof(TipoTransaccionEnum), v));
        }
    }
}
