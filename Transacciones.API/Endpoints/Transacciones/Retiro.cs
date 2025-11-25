using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Transacciones.Core.Exceptions;
using Transacciones.Core.Interfaces;
using Transacciones.Core.SharedKernel.Interfaces;

namespace Transacciones.API.Endpoints.Transacciones
{

    [ServiceFilter(typeof(Filters.ApiKeyAuthorizationFilter))]
    public class Retiro : EndpointBaseAsync
        .WithRequest<RetiroRequest>
        .WithActionResult<RetiroResponse>
    {
        private readonly ITransaccionService _transaccionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RetiroRequest> _validator;

        public Retiro(ITransaccionService transaccionService, IUnitOfWork unitOfWork, IValidator<RetiroRequest> validator)
        {
            _transaccionService = transaccionService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        [HttpPost("/api/transacciones/retiro")]
        [SwaggerOperation(
            Summary = "Realizar retiro",
            Description = "Realiza un retiro de una cuenta específica.",
            OperationId = "Transaccion.Retiro",
            Tags = new[] { "Transacciones" })
        ]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(RetiroResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public override async Task<ActionResult<RetiroResponse>> HandleAsync([FromBody] RetiroRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException(validationResult.ToString());
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var resultado = await _transaccionService.RealizarRetiroAsync(request.CuentaId, request.Monto, request.Descripcion, cancellationToken);
                await _unitOfWork.CommitAsync();

                return Ok(new RetiroResponse
                {
                    SaldoAnterior = resultado.SaldoAnterior,
                    NuevoSaldo = resultado.NuevoSaldo,
                    Mensaje = resultado.Mensaje
                });
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
