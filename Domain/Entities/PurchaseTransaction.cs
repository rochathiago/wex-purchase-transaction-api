namespace TransactionExchangeService.Domain.Entities;

public class PurchaseTransaction(string description, DateTime transactionDate, decimal amountUsd)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Description { get; private set; } = description;
    public DateTime TransactionDate { get; private set; } = transactionDate;
    public decimal AmountUsd { get; private set; } = Math.Round(amountUsd, 2);
}