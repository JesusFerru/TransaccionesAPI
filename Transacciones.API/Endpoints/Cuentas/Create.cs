using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Transacciones.Core.Entities.CuentaAggregate;
using Transacciones.Core.Interfaces;

namespace Transacciones.API.Endpoints.Cuentas
{
    public class CreateCuentaRequest
    {
        public required string NumeroCuenta { get; set; }
        public required decimal SaldoInicial { get; set; }
        public required string Titular { get; set; }
    }

    public class CreateCuentaResponse
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public string Titular { get; set; } = string.Empty;
        public bool Activa { get; set; }
    }

    public class Create : EndpointBaseAsync
        .WithRequest<CreateCuentaRequest>
        .WithActionResult<CreateCuentaResponse>
    {
        private readonly ICuentaService _cuentaService;
        private readonly IMapper _mapper;

        public Create(ICuentaService cuentaService, IMapper mapper)
        {
            _cuentaService = cuentaService;
            _mapper = mapper;
        }

        [HttpPost("/api/cuentas")]
        [SwaggerOperation(
            Summary = "Crear nueva cuenta",
            Description = "Crea una nueva cuenta bancaria con un saldo inicial.",
            OperationId = "Cuenta.Create",
            Tags = new[] { "Cuentas" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CreateCuentaResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public override async Task<ActionResult<CreateCuentaResponse>> HandleAsync([FromBody] CreateCuentaRequest request, CancellationToken cancellationToken = default)
        {
            var cuenta = _mapper.Map<Cuenta>(request);

            cuenta.FechaCreacion = DateTime.UtcNow;
            cuenta.Activa = true;
            cuenta.Saldo = request.SaldoInicial;

            var createdCuenta = await _cuentaService.CrearCuentaAsync(cuenta);
            var response = _mapper.Map<CreateCuentaResponse>(createdCuenta);

            return CreatedAtRoute("GetCuentaById", new { Id = response.Id }, response);

        }
    }
}
