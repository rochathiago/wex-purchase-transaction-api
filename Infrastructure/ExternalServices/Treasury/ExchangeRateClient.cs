using Microsoft.Extensions.Options;
using PurchaseTransactionAPI.Application.Exceptions;
using PurchaseTransactionAPI.Application.Interfaces;
using PurchaseTransactionAPI.Application.ExternalServices.Treasury.Options;
using System.Globalization;
using System.Text.Json;

namespace PurchaseTransactionAPI.Application.ExternalServices.Treasury;

public class ExchangeRateClient(IHttpClientFactoryCustom _httpClientFactory,
    ICurrencyRepository _currencyRepository,
    IOptions<ExchangeRateClientOptions> _options) : IExchangeRateClient
{

    public async Task<decimal?> GetExchangeRateAsync(string currencyCode, DateTime transactionDate)
    {

        var currency = await _currencyRepository.GetByCodeAsync(currencyCode);

        if (currency is null)
            throw new CurrencyNotSupportedException("Unsupported currency");

        var uriBuilder = new UriBuilder($"{_options.Value.BaseUrl}/{_options.Value.EndpointUrl}");

        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        query["fields"] = "country_currency_desc,exchange_rate,record_date";
        //query["filter"] = $"country_currency_desc:eq:{currency.Description},record_date:lte:{transactionDate:yyyy-MM-dd},record_date:gte:{transactionDate.AddMonths(-6):yyyy-MM-dd}";
        query["filter"] = $"country_currency_desc:eq:{currency.Description},record_date:lte:{transactionDate:yyyy-MM-dd}";
        query["sort"] = "-record_date";
        query["page[size]"] = "1";
        uriBuilder.Query = query.ToString();
        var url = uriBuilder.ToString();

        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode) throw new ExternalServiceException("Failed to retrieve exchange rate from external API.");

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<ExchangeRateResponse>(json, options);

        var item = result?.Data?.FirstOrDefault();
        if (item == null) throw new ExchangeRateNotFoundException("No exchange rate data found.");

        if (string.IsNullOrWhiteSpace(item.ExchangeRate)) throw new ExchangeRateNotFoundException("Exchange rate is missing.");

        if (item.RecordDate <= transactionDate.AddMonths(-6)) throw new System.InvalidOperationException("No exchange rate found within 6 months range.");

        if (!decimal.TryParse(item.ExchangeRate, NumberStyles.Any, CultureInfo.InvariantCulture, out var rate)) throw new ExternalServiceException("Invalid exchange rate format.");

        var rateString = result?.Data?.FirstOrDefault()?.ExchangeRate;

        if (string.IsNullOrEmpty(rateString))
            return null;

        return decimal.Parse(rateString, System.Globalization.CultureInfo.InvariantCulture);
    }
}
