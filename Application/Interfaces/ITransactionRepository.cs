using PurchaseTransactionAPI.Domain.Entities;
using PurchaseTransactionAPI.API.DTOs.Requests;
using PurchaseTransactionAPI.API.DTOs.Responses;

namespace PurchaseTransactionAPI.Application.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<Transaction?> GetByIdAsync(Guid id);
}