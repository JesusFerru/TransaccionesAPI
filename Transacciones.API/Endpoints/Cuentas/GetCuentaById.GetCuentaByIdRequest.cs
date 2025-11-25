using Microsoft.AspNetCore.Mvc;

namespace Transacciones.API.Endpoints.Cuentas
{
    public class GetCuentaByIdRequest
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }
}
