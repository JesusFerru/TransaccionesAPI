using FluentValidation;

namespace Transacciones.API.Endpoints.Cuentas
{
    public class CreateCuentaRequestValidator : AbstractValidator<CreateCuentaRequest>
    {
        public CreateCuentaRequestValidator()
        {
            RuleFor(x => x.NumeroCuenta)
                .NotEmpty().WithMessage("El número de cuenta es obligatorio.")
                .Length(10, 20).WithMessage("El número de cuenta debe tener entre 10 y 20 caracteres.");

            RuleFor(x => x.Titular)
                .NotEmpty().WithMessage("El titular es obligatorio.")
                .MaximumLength(100).WithMessage("El titular no puede exceder los 100 caracteres.");

            RuleFor(x => x.SaldoInicial)
                .GreaterThanOrEqualTo(0).WithMessage("El saldo inicial no puede ser negativo.");
        }
    }
}
