using PurchaseTransactionAPI.Domain.Entities;

namespace PurchaseTransactionAPI.Application.Interfaces;

public interface ICurrencyRepository
{
    public Task<Currency?> GetByCodeAsync(string currencyCode);
}