using Transacciones.Core.Entities.CuentaAggregate;
using Transacciones.Core.Exceptions;
using Transacciones.Core.Interfaces;
using Transacciones.Core.SharedKernel.Interfaces;

namespace Transacciones.Infrastructure.Services
{
    public class CuentaService : ICuentaService
    {
        private readonly IRepository<Cuenta> _repository;

        public CuentaService(IRepository<Cuenta> repository)
        {
            _repository = repository;
        }

        public async Task<Cuenta> CrearCuentaAsync(Cuenta cuenta, CancellationToken cancellationToken = default)
        {
            await _repository.AddAsync(cuenta, cancellationToken);
            return cuenta;
        }

        public async Task<Cuenta> ObtenerCuentaPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var cuenta = await _repository.GetByIdAsync(id, cancellationToken);

            if (cuenta == null)
                throw new NotFoundException($"No se encontró la cuenta con ID {id}");

            return cuenta;
        }
    }
}
