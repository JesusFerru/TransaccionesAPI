using Transacciones.Core.Entities.CuentaAggregate;

namespace Transacciones.Core.Interfaces
{
    public interface ICuentaService
    {
        Task<Cuenta> CrearCuentaAsync(Cuenta cuenta);
        Task<Cuenta> ObtenerCuentaPorIdAsync(int id);
    }
}
