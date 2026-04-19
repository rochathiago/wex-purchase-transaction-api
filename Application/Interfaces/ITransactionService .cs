using PurchaseTransactionAPI.Domain.Entities;
using PurchaseTransactionAPI.API.DTOs.Requests;
using PurchaseTransactionAPI.API.DTOs.Responses;

namespace PurchaseTransactionAPI.Application.Interfaces;

public interface ITransactionService
{
    public Task<TransactionResponse> CreateAsync(CreateTransactionRequest request);
    public Task<ConvertedTransactionResponse> GetConvertedAsync(Guid id, string currencyDescription);
}
