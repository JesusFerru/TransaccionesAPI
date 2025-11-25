using FluentValidation;
using Transacciones.API.Endpoints.Transacciones;

namespace Transacciones.API.Validators
{
    public class AbonoRequestValidator : AbstractValidator<AbonoRequest>
    {
        public AbonoRequestValidator()
        {
            RuleFor(x => x.CuentaId)
                .GreaterThan(0).WithMessage("El ID de la cuenta debe ser válido.");

            RuleFor(x => x.Monto)
                .GreaterThan(0).WithMessage("El monto del abono debe ser mayor a 0.");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(200).WithMessage("La descripción no puede exceder los 200 caracteres.");
        }
    }
}
