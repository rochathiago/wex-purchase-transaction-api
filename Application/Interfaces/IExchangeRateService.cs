using PurchaseTransactionAPI.Application.Responses;

namespace PurchaseTransactionAPI.Application.Interfaces;

public interface IExchangeRateService
{
    Task<ExchangeRateResult> GetRateAsync(string currency, DateTime transactionDate);
}