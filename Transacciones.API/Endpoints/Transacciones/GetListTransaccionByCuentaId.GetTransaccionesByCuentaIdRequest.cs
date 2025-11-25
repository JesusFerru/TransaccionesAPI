using Microsoft.AspNetCore.Mvc;

namespace Transacciones.API.Endpoints.Transacciones
{
    public class GetTransaccionesByCuentaIdRequest
    {
        [FromRoute(Name = "id")]
        public int CuentaId { get; set; }
    }
}
