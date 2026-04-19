using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using PurchaseTransactionAPI.Application.ExternalServices;
using PurchaseTransactionAPI.Application.ExternalServices.Treasury;
using PurchaseTransactionAPI.Application.ExternalServices.Treasury.Options;
using PurchaseTransactionAPI.Application.Interfaces;
using PurchaseTransactionAPI.Domain.Entities;

namespace PurchaseTransaction.Tests.IntegrationTests.Infrastructure.ExternalServices.Treasury
{
    public class ExchangeRateClientTests
    {
        private readonly IHttpClientFactoryCustom _httpClientFactoryCustom;
        private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;
        private readonly IOptions<ExchangeRateClientOptions> _options;
        private readonly ExchangeRateClient _exchangeRateClient;
       
        public ExchangeRateClientTests()
        {
            _httpClientFactoryCustom = new HttpClientFactoryCustom();
            _currencyRepositoryMock = new Mock<ICurrencyRepository>();
            _options = Options.Create(new ExchangeRateClientOptions() { 
                BaseUrl = "https://api.fiscaldata.treasury.gov",
                EndpointUrl = "services/api/fiscal_service/v1/accounting/od/rates_of_exchange"});

            _exchangeRateClient = new ExchangeRateClient(_httpClientFactoryCustom, _currencyRepositoryMock.Object, _options);
        }


        [Fact]
        public async Task GetExchangeRateAsync_Should_Return_Valid_Rate()
        {
            // Arrange            
            var currency = "BRL";
            var currencyDescription = "Brazil-Real";
            var date = DateTime.UtcNow;

            _currencyRepositoryMock.Setup(r => r.GetByCodeAsync(currency))
                .ReturnsAsync(new Currency(currency, currencyDescription));

            // Act
            var rate = await _exchangeRateClient.GetExchangeRateAsync(currency, date);
            // Assert
            rate.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetRateAsync_With_No_Currency_Conversion_Rate_Available_Within_6_Months_Should_Return_InvalidOperationException()
        {
            // Arrange
            var currency = "BRL";
            var currencyDescription = "Brazil-Real";
            _currencyRepositoryMock.Setup(r => r.GetByCodeAsync(currency))
                .ReturnsAsync(new Currency(currency, currencyDescription));

            // Act
            Func<Task> act = async () => { await _exchangeRateClient.GetExchangeRateAsync(currency, DateTime.Now.AddMonths(7)); };
            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("No exchange rate found within 6 months range.");
        }
    }
}
