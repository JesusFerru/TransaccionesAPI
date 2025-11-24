namespace Transacciones.Core.Entities.TransaccionAggregate
{
    public class Transaccion
    {
        public required int Id { get; set; }
        public required int CuentaId { get; set; }
        public required string TipoTransaccion { get; set; } // ABONO o RETIRO 
        public required decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public required string Descripcion { get; set; }
    }
}
