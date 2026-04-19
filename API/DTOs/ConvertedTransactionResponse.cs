namespace PurchaseTransactionAPI.API.DTOs.Responses;

public class ConvertedTransactionResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal OriginalAmountUsd { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal ConvertedAmount { get; set; }
    public string CurrencyCode { get; set; }
}