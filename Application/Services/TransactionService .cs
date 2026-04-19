using PurchaseTransactionAPI.Application.Interfaces;
using PurchaseTransactionAPI.API.DTOs.Requests;
using PurchaseTransactionAPI.API.DTOs.Responses;
using PurchaseTransactionAPI.Domain.Entities;

namespace PurchaseTransactionAPI.Application.Services;

public class TransactionService(
    ITransactionRepository _repository, 
    IExchangeRateService _exchangeRateService) : ITransactionService
{
    public async Task<TransactionResponse> CreateAsync(CreateTransactionRequest request)
    {
        var transaction = new Transaction(
            request.Description,
            request.TransactionDate,
            request.Amount
        );

        await _repository.AddAsync(transaction);

        return new TransactionResponse
        {
            Id = transaction.Id,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            AmountUsd = transaction.AmountUsd
        };
    }

    public async Task<ConvertedTransactionResponse> GetConvertedAsync(Guid id, string currencyCode)
    {

        var transaction = await _repository.GetByIdAsync(id);

        if (transaction == null)
            throw new KeyNotFoundException("Transaction not found");
        
        var exchangeRate = await _exchangeRateService.GetRateAsync(currencyCode, transaction.TransactionDate);

        var convertedAmount = Math.Round(transaction.AmountUsd * exchangeRate.Rate.Value, 2);

        return new ConvertedTransactionResponse
        {
            Id = transaction.Id,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            OriginalAmountUsd = transaction.AmountUsd,
            ExchangeRate = exchangeRate.Rate.Value,
            ConvertedAmount = convertedAmount,
            CurrencyCode = currencyCode
        };

    }
}