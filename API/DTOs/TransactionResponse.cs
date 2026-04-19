namespace PurchaseTransactionAPI.API.DTOs.Responses;

public class TransactionResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; } = String.Empty;
    public DateTime TransactionDate { get; set; }
    public decimal AmountUsd { get; set; }
}