namespace PurchaseTransactionAPI.API.DTOs.Requests;

public class CreateTransactionRequest
{
    public string Description { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
}