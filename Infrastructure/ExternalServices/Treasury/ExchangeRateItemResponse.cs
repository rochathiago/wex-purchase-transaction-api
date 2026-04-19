using System.Text.Json.Serialization;

namespace PurchaseTransactionAPI.Application.ExternalServices.Treasury;

public class ExchangeRateItemResponse
{
    [JsonPropertyName("country_currency_desc")]
    public string CurrencyDescription { get; set; }

    [JsonPropertyName("exchange_rate")]
    public string ExchangeRate { get; set; }

    [JsonPropertyName("record_date")]
    public DateTime RecordDate { get; set; }
}
