using FluentValidation;
using Transacciones.API.Endpoints.Cuentas;

namespace Transacciones.API.Validators
{
    public class CreateCuentaValidator : AbstractValidator<CreateCuentaRequest>
    {
        public CreateCuentaValidator()
        {
            RuleFor(x => x.NumeroCuenta)
                .NotEmpty().WithMessage("El número de cuenta es obligatorio.")
                .MaximumLength(20).WithMessage("El número de cuenta no puede superar los 20 caracteres.");

            RuleFor(x => x.Titular)
                .NotEmpty().WithMessage("El titular es obligatorio.")
                .MaximumLength(100);

            RuleFor(x => x.SaldoInicial)
                .GreaterThanOrEqualTo(0).WithMessage("El saldo inicial debe ser mayor a 0.");
        }
    }
}
