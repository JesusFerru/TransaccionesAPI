using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Transacciones.Core.Interfaces;

namespace Transacciones.API.Endpoints.Transacciones
{

    public class GetListTransaccionByCuentaId : EndpointBaseAsync
        .WithRequest<GetTransaccionesByCuentaIdRequest>
        .WithActionResult<IEnumerable<TransaccionResponse>>
    {
        private readonly ITransaccionService _transaccionService;
        private readonly IMapper _mapper;

        public GetListTransaccionByCuentaId(ITransaccionService transaccionService, IMapper mapper)
        {
            _transaccionService = transaccionService;
            _mapper = mapper;
        }

        [HttpGet("/api/transacciones/cuenta/{id}")]
        [SwaggerOperation(
            Summary = "Obtener historial de transacciones",
            Description = "Obtiene el historial de transacciones de una cuenta específica.",
            OperationId = "Transaccion.GetByCuentaId",
            Tags = new[] { "Transacciones" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TransaccionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public override async Task<ActionResult<IEnumerable<TransaccionResponse>>> HandleAsync([FromRoute] GetTransaccionesByCuentaIdRequest request, CancellationToken cancellationToken = default)
        {
            var transacciones = await _transaccionService.ObtenerHistorialAsync(request.CuentaId, cancellationToken);

            var response = _mapper.Map<IEnumerable<TransaccionResponse>>(transacciones);
            return Ok(response);
        }
    }
}
