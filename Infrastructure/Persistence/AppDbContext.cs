using Microsoft.EntityFrameworkCore;
using PurchaseTransactionAPI.Domain.Entities;

namespace PurchaseTransactionAPI.Application.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Currency> Currencies { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.AmountUsd)
                .HasPrecision(18, 2);

            entity.Property(x => x.TransactionDate)
                .IsRequired();
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(x => x.Code);

            entity.Property(x => x.Code)
                .HasMaxLength(3)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(100)
                .IsRequired();
        });
    }
}
