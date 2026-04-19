using PurchaseTransactionAPI.Application.Interfaces;

namespace PurchaseTransactionAPI.Application.ExternalServices;

public class HttpClientFactoryCustom : IHttpClientFactoryCustom
{
    public HttpClient CreateClient()
    {
        var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.Add("User-Agent", "PurchaseApp");
        return client;
    }
}
