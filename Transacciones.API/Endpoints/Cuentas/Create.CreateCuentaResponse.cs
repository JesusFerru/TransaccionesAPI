namespace Transacciones.API.Endpoints.Cuentas
{
    public class CreateCuentaResponse
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public string Titular { get; set; } = string.Empty;
        public bool Activa { get; set; }
    }
}
