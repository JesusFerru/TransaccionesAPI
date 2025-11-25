namespace Transacciones.API.Endpoints.Cuentas
{
    public class CreateCuentaRequest
    {
        public required string NumeroCuenta { get; set; }
        public required decimal SaldoInicial { get; set; }
        public required string Titular { get; set; }
    }
}
