using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transacciones.Core.Entities.CuentaAggregate;

namespace Transacciones.Infrastructure.Data.Configuration
{
    public class CuentaConfiguration : IEntityTypeConfiguration<Cuenta>
    {
        public void Configure(EntityTypeBuilder<Cuenta> builder)
        {
            builder.ToTable(nameof(Cuenta));

            builder.HasKey(t => t.Id);
            builder.HasIndex(t => t.NumeroCuenta)
                .IsUnique();
            builder.Property(t => t.NumeroCuenta)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(t => t.Titular)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(t => t.Saldo)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

        }
    }
}
