using Moq;
using Transacciones.Core.Entities.CuentaAggregate;
using Transacciones.Core.Entities.TransaccionAggregate;
using Transacciones.Core.Exceptions;
using Transacciones.Core.SharedKernel.Interfaces;
using Transacciones.Infrastructure.Services;

namespace Transacciones.Tests.Core.Services
{
    public class TransaccionServiceTests
    {
        private readonly Mock<IRepository<Cuenta>> _mockCuentaRepository;
        private readonly Mock<IRepository<Transaccion>> _mockTransaccionRepository;
        private readonly TransaccionService _transaccionService;

        public TransaccionServiceTests()
        {
            _mockCuentaRepository = new Mock<IRepository<Cuenta>>();
            _mockTransaccionRepository = new Mock<IRepository<Transaccion>>();
            _transaccionService = new TransaccionService(_mockCuentaRepository.Object, _mockTransaccionRepository.Object);
        }

        [Fact]
        public async Task RealizarAbonoAsync_CuentaExiste_DebeIncrementarSaldoYRetornarExito()
        {
            // Arrange
            var cuentaId = 1;
            var monto = 100m;
            var saldoInicial = 50m;
            var cuenta = new Cuenta
            {
                NumeroCuenta = "1234567890",
                Saldo = saldoInicial,
                Titular = "Test User",
                Activa = true,
                FechaCreacion = DateTime.UtcNow
            };

            typeof(Transacciones.Core.SharedKernel.BaseEntity).GetProperty("Id")?.SetValue(cuenta, cuentaId);

            _mockCuentaRepository.Setup(r => r.GetByIdAsync(cuentaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cuenta);

            // Act
            var resultado = await _transaccionService.RealizarAbonoAsync(cuentaId, monto, "Abono de prueba", CancellationToken.None);

            // Assert
            Assert.Equal(saldoInicial + monto, cuenta.Saldo);
            Assert.Equal(saldoInicial + monto, resultado.NuevoSaldo);
            Assert.Equal("Abono realizado exitosamente.", resultado.Mensaje);

            _mockCuentaRepository.Verify(r => r.UpdateAsync(cuenta, It.IsAny<CancellationToken>()), Times.Once);
            _mockTransaccionRepository.Verify(r => r.AddAsync(It.Is<Transaccion>(t =>
                t.CuentaId == cuentaId &&
                t.Monto == monto &&
                t.TipoTransaccion == TipoTransaccionEnum.ABONO), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RealizarAbonoAsync_CuentaNoExiste_DebeLanzarKeyNotFoundException()
        {
            // Arrange
            var cuentaId = 99;
            _mockCuentaRepository.Setup(r => r.GetByIdAsync(cuentaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cuenta?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _transaccionService.RealizarAbonoAsync(cuentaId, 100m, "Abono fallido", CancellationToken.None));

            _mockCuentaRepository.Verify(r => r.UpdateAsync(It.IsAny<Cuenta>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockTransaccionRepository.Verify(r => r.AddAsync(It.IsAny<Transaccion>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RealizarRetiroAsync_CuentaExisteYSaldoSuficiente_DebeDecrementarSaldoYRetornarExito()
        {
            // Arrange
            var cuentaId = 1;
            var monto = 50m;
            var saldoInicial = 100m;
            var cuenta = new Cuenta
            {
                NumeroCuenta = "1234567890",
                Saldo = saldoInicial,
                Titular = "Test User",
                Activa = true,
                FechaCreacion = DateTime.UtcNow
            };
            typeof(Transacciones.Core.SharedKernel.BaseEntity).GetProperty("Id")?.SetValue(cuenta, cuentaId);

            _mockCuentaRepository.Setup(r => r.GetByIdAsync(cuentaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cuenta);

            // Act
            var resultado = await _transaccionService.RealizarRetiroAsync(cuentaId, monto, "Retiro de prueba", CancellationToken.None);

            // Assert
            Assert.Equal(saldoInicial - monto, cuenta.Saldo);
            Assert.Equal(saldoInicial - monto, resultado.NuevoSaldo);
            Assert.Equal("Retiro realizado exitosamente.", resultado.Mensaje);

            _mockCuentaRepository.Verify(r => r.UpdateAsync(cuenta, It.IsAny<CancellationToken>()), Times.Once);
            _mockTransaccionRepository.Verify(r => r.AddAsync(It.Is<Transaccion>(t =>
                t.CuentaId == cuentaId &&
                t.Monto == monto &&
                t.TipoTransaccion == TipoTransaccionEnum.RETIRO), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RealizarRetiroAsync_CuentaNoExiste_DebeLanzarKeyNotFoundException()
        {
            // Arrange
            var cuentaId = 99;
            _mockCuentaRepository.Setup(r => r.GetByIdAsync(cuentaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cuenta?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _transaccionService.RealizarRetiroAsync(cuentaId, 50m, "Retiro fallido", CancellationToken.None));
        }

        [Fact]
        public async Task RealizarRetiroAsync_SaldoInsuficiente_DebeLanzarBadRequestException()
        {
            // Arrange
            var cuentaId = 1;
            var monto = 100m;
            var saldoInicial = 50m;
            var cuenta = new Cuenta
            {
                NumeroCuenta = "1234567890",
                Saldo = saldoInicial,
                Titular = "Test User",
                Activa = true,
                FechaCreacion = DateTime.UtcNow
            };
            typeof(Transacciones.Core.SharedKernel.BaseEntity).GetProperty("Id")?.SetValue(cuenta, cuentaId);

            _mockCuentaRepository.Setup(r => r.GetByIdAsync(cuentaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cuenta);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                _transaccionService.RealizarRetiroAsync(cuentaId, monto, "Retiro fallido", CancellationToken.None));

            Assert.Equal("Saldo insuficiente para realizar el retiro.", exception.Message);

            _mockCuentaRepository.Verify(r => r.UpdateAsync(It.IsAny<Cuenta>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockTransaccionRepository.Verify(r => r.AddAsync(It.IsAny<Transaccion>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
