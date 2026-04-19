namespace PurchaseTransactionAPI.Application.Interfaces;

public interface IExchangeRateClient
{
    public Task<decimal?> GetExchangeRateAsync(string currencyCode, DateTime transactionDate);
}
