using Transacciones.Core.Entities.CuentaAggregate;
using Transacciones.Core.Entities.Dtos;
using Transacciones.Core.Entities.TransaccionAggregate;
using Transacciones.Core.Entities.TransaccionAggregate.Specifications;
using Transacciones.Core.Exceptions;
using Transacciones.Core.Interfaces;
using Transacciones.Core.SharedKernel.Interfaces;

namespace Transacciones.Infrastructure.Services
{
    public class TransaccionService : ITransaccionService
    {
        private readonly IRepository<Cuenta> _cuentaRepository;
        private readonly IRepository<Transaccion> _transaccionRepository;

        public TransaccionService(IRepository<Cuenta> cuentaRepository, IRepository<Transaccion> transaccionRepository)
        {
            _cuentaRepository = cuentaRepository;
            _transaccionRepository = transaccionRepository;
        }

        public async Task<ResultadoTransaccion> RealizarAbonoAsync(int cuentaId, decimal monto, string descripcion, CancellationToken cancellationToken)
        {
            var cuenta = await _cuentaRepository.GetByIdAsync(cuentaId, cancellationToken);
            if (cuenta == null)
            {
                throw new NotFoundException($"No se encontró la cuenta con ID {cuentaId}");
            }

            var saldoAnterior = cuenta.Saldo;
            cuenta.Saldo += monto;
            await _cuentaRepository.UpdateAsync(cuenta, cancellationToken);

            var transaccion = new Transaccion
            {
                CuentaId = cuentaId,
                TipoTransaccion = TipoTransaccionEnum.ABONO,
                Monto = monto,
                FechaTransaccion = DateTime.UtcNow,
                Descripcion = descripcion,
                Cuenta = cuenta
            };

            await _transaccionRepository.AddAsync(transaccion, cancellationToken);

            return new ResultadoTransaccion
            {
                SaldoAnterior = saldoAnterior,
                NuevoSaldo = cuenta.Saldo,
                Mensaje = "Abono realizado exitosamente."
            };
        }

        public async Task<ResultadoTransaccion> RealizarRetiroAsync(int cuentaId, decimal monto, string descripcion, CancellationToken cancellationToken)
        {
            var cuenta = await _cuentaRepository.GetByIdAsync(cuentaId, cancellationToken);
            if (cuenta == null)
            {
                throw new NotFoundException($"No se encontró la cuenta con ID {cuentaId}");
            }

            if (cuenta.Saldo < monto)
            {
                throw new BadRequestException("Saldo insuficiente para realizar el retiro.");
            }

            var saldoAnterior = cuenta.Saldo;
            cuenta.Saldo -= monto;
            await _cuentaRepository.UpdateAsync(cuenta, cancellationToken);

            var transaccion = new Transaccion
            {
                CuentaId = cuentaId,
                TipoTransaccion = TipoTransaccionEnum.RETIRO,
                Monto = monto,
                FechaTransaccion = DateTime.UtcNow,
                Descripcion = descripcion,
                Cuenta = cuenta
            };

            await _transaccionRepository.AddAsync(transaccion, cancellationToken);

            return new ResultadoTransaccion
            {
                SaldoAnterior = saldoAnterior,
                NuevoSaldo = cuenta.Saldo,
                Mensaje = "Retiro realizado exitosamente."
            };
        }

        public async Task<IEnumerable<Transaccion>> ObtenerHistorialAsync(int cuentaId, CancellationToken cancellationToken)
        {
            var spec = new TransaccionesPorCuentaSpec(cuentaId);

            return await _transaccionRepository.ListAsync(spec, cancellationToken);
        }
    }
}
