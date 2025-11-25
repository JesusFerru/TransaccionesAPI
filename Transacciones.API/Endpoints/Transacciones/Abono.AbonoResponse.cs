namespace Transacciones.API.Endpoints.Transacciones
{
    public class AbonoResponse
    {
        public decimal SaldoAnterior { get; set; }
        public decimal NuevoSaldo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
