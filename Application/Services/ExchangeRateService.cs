using PurchaseTransactionAPI.Application.Interfaces;
using PurchaseTransactionAPI.Application.Responses;

namespace PurchaseTransactionAPI.Application.Services;

public class ExchangeRateService(IExchangeRateClient _exchangeRateClient) : IExchangeRateService
{
    public async Task<ExchangeRateResult?> GetRateAsync(string currencyCode, DateTime transactionDate)
    {        
        var exchangeRate = await _exchangeRateClient.GetExchangeRateAsync(currencyCode, transactionDate);

        return new ExchangeRateResult
        {
            Rate = exchangeRate,
            RateDate = transactionDate
        };
    }
}