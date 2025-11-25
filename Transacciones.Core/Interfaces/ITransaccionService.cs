using Transacciones.Core.Entities.TransaccionAggregate;

namespace Transacciones.Core.Interfaces
{
    public class ResultadoTransaccion
    {
        public int TransaccionId { get; set; }
        public decimal NuevoSaldo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }

    public interface ITransaccionService
    {
        Task<ResultadoTransaccion> RealizarAbonoAsync(int cuentaId, decimal monto, string descripcion, CancellationToken cancellationToken);
        Task<ResultadoTransaccion> RealizarRetiroAsync(int cuentaId, decimal monto, string descripcion, CancellationToken cancellationToken);
        Task<IEnumerable<Transaccion>> ObtenerHistorialAsync(int cuentaId, CancellationToken cancellationToken);
    }
}
