using Transacciones.Core.Entities.CuentaAggregate;
using Transacciones.Core.SharedKernel;

namespace Transacciones.Core.Entities.TransaccionAggregate
{
    public class Transaccion : BaseEntity
    {
        public required int CuentaId { get; set; }
        public required string TipoTransaccion { get; set; } // ABONO o RETIRO 
        public required decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public required string Descripcion { get; set; }
        public required Cuenta Cuenta { get; set; }
    }
}
