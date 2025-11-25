using Transacciones.Core.Entities.CuentaAggregate;

namespace Transacciones.Core.Interfaces
{
    public interface ICuentaService
    {
        Task<Cuenta> CrearCuentaAsync(Cuenta cuenta, CancellationToken cancellation);
        Task<Cuenta> ObtenerCuentaPorIdAsync(int id, CancellationToken cancellation);
    }
}
