namespace Transacciones.Core.Exceptions
{
    public class AccountNotFoundException : NotFoundException
    {
        public AccountNotFoundException(Guid cuentaId)
            : base($"La cuenta con ID '{cuentaId}' no existe.")
        {
        }
    }

    public class InsufficientBalanceException : BadRequestException
    {
        public InsufficientBalanceException(decimal solicitado, decimal disponible)
            : base($"Saldo insuficiente. Monto solicitado: {solicitado}. Saldo disponible: {disponible}.")
        {
        }
    }
    public class BusinessRuleException : ConflictException
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}
