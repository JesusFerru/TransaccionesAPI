using Ardalis.Specification;

namespace Transacciones.Core.Entities.TransaccionAggregate.Specifications
{
    public class TransaccionesPorCuentaSpec : Specification<Transaccion>
    {
        public TransaccionesPorCuentaSpec(int cuentaId)
        {
            Query.Where(t => t.CuentaId == cuentaId)
                 .OrderByDescending(t => t.FechaTransaccion);
        }
    }
}
