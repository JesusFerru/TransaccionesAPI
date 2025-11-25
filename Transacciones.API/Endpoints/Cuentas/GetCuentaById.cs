using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Transacciones.Core.Interfaces;

namespace Transacciones.API.Endpoints.Cuentas
{

    [ServiceFilter(typeof(Filters.ApiKeyAuthorizationFilter))]
    public class GetCuentaById : EndpointBaseAsync
        .WithRequest<GetCuentaByIdRequest>
        .WithActionResult<CuentaResponse>
    {
        private readonly ICuentaService _cuentaService;
        private readonly IMapper _mapper;

        public GetCuentaById(ICuentaService cuentaService, IMapper mapper)
        {
            _cuentaService = cuentaService;
            _mapper = mapper;
        }

        [HttpGet("/api/cuentas/{id}")]
        [SwaggerOperation(
            Summary = "Obtener cuenta por ID",
            Description = "Obtiene los detalles de una cuenta específica por su ID.",
            OperationId = "Cuenta.GetCuentaById",
            Tags = new[] { "Cuentas" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CuentaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public override async Task<ActionResult<CuentaResponse>> HandleAsync([FromRoute] GetCuentaByIdRequest request, CancellationToken cancellationToken = default)
        {
            var cuenta = await _cuentaService.ObtenerCuentaPorIdAsync(request.Id, cancellationToken);
            var response = _mapper.Map<CuentaResponse>(cuenta);
            return Ok(response);
        }
    }
}
