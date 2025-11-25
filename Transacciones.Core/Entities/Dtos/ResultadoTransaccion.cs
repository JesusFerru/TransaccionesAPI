namespace Transacciones.Core.Entities.Dtos
{
    public class ResultadoTransaccion
    {
        public decimal SaldoAnterior { get; set; }
        public decimal NuevoSaldo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
