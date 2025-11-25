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
            builder.Property(t => t.CuentaId)
                .IsRequired();

            builder.Property(t => t.Monto)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(t => t.TipoTransaccion)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(t => t.Cuenta)
                .WithMany(c => c.Transacciones)
                .HasForeignKey(t => t.CuentaId);

        }
    }
}
