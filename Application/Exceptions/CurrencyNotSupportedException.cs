namespace PurchaseTransactionAPI.Application.Exceptions
{
    public class CurrencyNotSupportedException : Exception
    {
        public CurrencyNotSupportedException(string message) : base(message) { }
    }
}
