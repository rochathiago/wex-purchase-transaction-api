using PurchaseTransactionAPI.Application.Persistence;
using PurchaseTransactionAPI.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using PurchaseTransactionAPI.Domain.Entities;

namespace PurchaseTransactionAPI.Application.Repositories;

public class TransactionRepository(AppDbContext _context) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
