using Transacciones.Core.Entities.Dtos;
using Transacciones.Core.Entities.TransaccionAggregate;

namespace Transacciones.Core.Interfaces
{
    public interface ITransaccionService
    {
        Task<ResultadoTransaccion> RealizarAbonoAsync(int cuentaId, decimal monto, string descripcion, CancellationToken cancellationToken);
        Task<ResultadoTransaccion> RealizarRetiroAsync(int cuentaId, decimal monto, string descripcion, CancellationToken cancellationToken);
        Task<IEnumerable<Transaccion>> ObtenerHistorialAsync(int cuentaId, CancellationToken cancellationToken);
    }
}
