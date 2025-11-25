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
    public class AbonoRequest
    {
        public int CuentaId { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class AbonoResponse
    {
        public int TransaccionId { get; set; }
        public decimal NuevoSaldo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }

    public class Abono : EndpointBaseAsync
        .WithRequest<AbonoRequest>
        .WithActionResult<AbonoResponse>
    {
        private readonly ITransaccionService _transaccionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AbonoRequest> _validator;

        public Abono(ITransaccionService transaccionService, IUnitOfWork unitOfWork, IValidator<AbonoRequest> validator)
        {
            _transaccionService = transaccionService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        [HttpPost("/api/transacciones/abono")]
        [SwaggerOperation(
            Summary = "Realizar abono",
            Description = "Realiza un abono a una cuenta específica.",
            OperationId = "Transaccion.Abono",
            Tags = new[] { "Transacciones" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AbonoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public override async Task<ActionResult<AbonoResponse>> HandleAsync([FromBody] AbonoRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException(validationResult.ToString());
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var resultado = await _transaccionService.RealizarAbonoAsync(request.CuentaId, request.Monto, request.Descripcion, cancellationToken);
                await _unitOfWork.CommitAsync();

                return Ok(new AbonoResponse
                {
                    TransaccionId = resultado.TransaccionId,
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
