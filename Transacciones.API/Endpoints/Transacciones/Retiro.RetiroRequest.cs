namespace Transacciones.API.Endpoints.Transacciones
{
    public class RetiroRequest
    {
        public int CuentaId { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
