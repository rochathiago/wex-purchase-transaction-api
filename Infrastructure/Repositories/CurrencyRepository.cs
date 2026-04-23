using Microsoft.EntityFrameworkCore;
using PurchaseTransactionAPI.Application.Interfaces;
using PurchaseTransactionAPI.Application.Persistence;
using PurchaseTransactionAPI.Domain.Entities;

namespace PurchaseTransactionAPI.Infrastructure.Repositories;

public class CurrencyRepository(AppDbContext _context) : ICurrencyRepository
{

    public async Task<Currency?> GetByCodeAsync(string code)
    {
        return await _context.Currencies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == code);
    }
}
