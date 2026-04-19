namespace PurchaseTransactionAPI.Application.Exceptions;

public class ExchangeRateNotFoundException : Exception
{
    public ExchangeRateNotFoundException(string message) : base(message) { }
}
