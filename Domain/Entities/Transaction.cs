namespace PurchaseTransactionAPI.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public decimal AmountUsd { get; private set; }

    private Transaction() { }

    public Transaction(string description, DateTime date, decimal amount)
    {

        if (string.IsNullOrWhiteSpace(description?.Trim()) || description.Length > 50)
            throw new ArgumentException("Invalid description");

        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");

        Id = Guid.NewGuid();
        Description = description;
        TransactionDate = date;
        AmountUsd = Math.Round(amount, 2);
    }
}
