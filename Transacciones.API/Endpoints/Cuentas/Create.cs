using Ardalis.ApiEndpoints;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Transacciones.Core.Entities.CuentaAggregate;
using Transacciones.Core.Exceptions;
using Transacciones.Core.Interfaces;
using Transacciones.Core.SharedKernel.Interfaces;

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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCuentaRequest> _validator;

        public Create(ICuentaService cuentaService, IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateCuentaRequest> validator)
        {
            _cuentaService = cuentaService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
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
        public override async Task<ActionResult<CreateCuentaResponse>> HandleAsync(
            [FromBody] CreateCuentaRequest request,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException(validationResult.ToString());
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var cuenta = _mapper.Map<Cuenta>(request);

                cuenta.FechaCreacion = DateTime.UtcNow;
                cuenta.Activa = true;
                cuenta.Saldo = request.SaldoInicial;

                var createdCuenta = await _cuentaService.CrearCuentaAsync(cuenta, cancellationToken);

                await _unitOfWork.CommitAsync();

                var response = _mapper.Map<CreateCuentaResponse>(createdCuenta);

                return Created($"/api/cuentas/{createdCuenta.Id}", response);

            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
