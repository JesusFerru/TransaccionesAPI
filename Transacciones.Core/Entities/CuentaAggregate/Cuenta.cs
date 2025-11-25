using Transacciones.Core.Entities.TransaccionAggregate;
using Transacciones.Core.SharedKernel;
using Transacciones.Core.SharedKernel.Interfaces;

namespace Transacciones.Core.Entities.CuentaAggregate
{
    public class Cuenta : BaseEntity, IAggregateRoot
    {
        public required string NumeroCuenta { get; set; }
        public required decimal Saldo { get; set; }
        public required string Titular { get; set; }
        public DateTime FechaCreacion { get; set; }
        public required bool Activa { get; set; }
        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();

    }
}
