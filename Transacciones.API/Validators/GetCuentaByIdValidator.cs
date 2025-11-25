using FluentValidation;
using Transacciones.API.Endpoints.Cuentas;

namespace Transacciones.API.Validators
{
    public class GetCuentaByIdValidator : AbstractValidator<GetCuentaByIdRequest>
    {
        public GetCuentaByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El ID debe ser un número positivo.");
        }
    }
}
