using Transacciones.Core.Entities.CuentaAggregate;
using Transacciones.Core.Exceptions;
using Transacciones.Core.Interfaces;
using Transacciones.Core.SharedKernel.Interfaces;

namespace Transacciones.Core.Services
{
    public class CuentaService : ICuentaService
    {
        private readonly IRepository<Cuenta> _repository;

        public CuentaService(IRepository<Cuenta> repository)
        {
            _repository = repository;
        }

        public async Task<Cuenta> CrearCuentaAsync(Cuenta cuenta)
        {
            // Validar si ya existe una cuenta con el mismo número (simulado o real si hay método en repo)
            // Aquí asumimos que el repositorio maneja la persistencia básica.
            // Podríamos agregar validaciones de negocio aquí.

            return await _repository.AddAsync(cuenta);
        }

        public async Task<Cuenta> ObtenerCuentaPorIdAsync(int id)
        {
            var cuenta = await _repository.GetByIdAsync(id);
            if (cuenta == null)
            {
                // La excepción será manejada por el middleware global
                throw new NotFoundException($"No se encontró la cuenta con ID {id}");
            }
            return cuenta;
        }
    }
}
