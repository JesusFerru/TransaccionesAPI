using Transacciones.Core.SharedKernel;

namespace Transacciones.Core.Entities.CuentaAggregate
{
    public class Cuenta : BaseEntity
    {
        public required string NumeroCuenta { get; set; }
        public required decimal Saldo { get; set; }
        public required string Titular { get; set; }
        public DateTime FechaCreacion { get; set; }
        public required bool Activa { get; set; }
    }
}
