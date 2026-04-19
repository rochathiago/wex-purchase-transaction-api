namespace PurchaseTransactionAPI.Application.Interfaces;

public interface IHttpClientFactoryCustom
{
    public HttpClient CreateClient();
}
