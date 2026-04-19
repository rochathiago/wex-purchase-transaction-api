using FluentAssertions;
using Moq;
using PurchaseTransactionAPI.Application.Interfaces;
using PurchaseTransactionAPI.Application.Services;

namespace PurchaseTransaction.Tests.UnitTests.Services;

public class ExchangeRateServiceTests
{
    private readonly Mock<IExchangeRateClient> _exchangeRateClientMock;
    private readonly ExchangeRateService _exchangeRateService;

    public ExchangeRateServiceTests()
    {
        _exchangeRateClientMock = new Mock<IExchangeRateClient>();
        _exchangeRateService = new ExchangeRateService(_exchangeRateClientMock.Object);
    }

    [Fact]
    public async Task GetRateAsync_With_Valid_Currency_Should_Return_Valid_ExchangeRateResult()
    {
        // Arrange
        var expectedRate = 1.25m;
        _exchangeRateClientMock.Setup(client => client.GetExchangeRateAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(expectedRate);
        var service = new ExchangeRateService(_exchangeRateClientMock.Object);
        var transactionDate = DateTime.UtcNow;
        
        // Act
        var result = await service.GetRateAsync("BRL", transactionDate);
        
        // Assert
        result.Should().NotBeNull();
        result.Rate.Should().Be(expectedRate);
        result.RateDate.Should().Be(transactionDate);
    }


    [Fact]
    public async Task GetRateAsync_With_Invalid_Currency_Should_Throw_ArgumentException()
    {
        // Arrange
        _exchangeRateClientMock.Setup(s => s.GetExchangeRateAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ThrowsAsync(new ArgumentException("Invalid currency code"));

        // Act
        Func<Task> act = async () => { await _exchangeRateService.GetRateAsync("XYZ", new DateTime()); };

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid currency code");
    }

    [Fact]
    public async Task GetRateAsync_With_ExternalServiceFailure_Should_Throw_Exception()
    {
        // Arrange
        _exchangeRateClientMock.Setup(s => s.GetExchangeRateAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ThrowsAsync(new Exception("External service failure"));
        // Act
        Func<Task> act = async () => { await _exchangeRateService.GetRateAsync("BRL", new DateTime()); };
        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("External service failure");
    }

    [Fact]
    public async Task GetRateAsync_With_Null_Response_Should_Return_Null()
    {
        // Arrange
        _exchangeRateClientMock.Setup(s => s.GetExchangeRateAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync((decimal?)null);
        // Act
        var result = await _exchangeRateService.GetRateAsync("BRL", new DateTime());
        // Assert
        result.Should().NotBeNull();
        result.Rate.Should().BeNull();
    }

    [Fact]
    public async Task GetRateAsync_With_Zero_Rate_Should_Return_Zero()
    {
        // Arrange
        _exchangeRateClientMock.Setup(s => s.GetExchangeRateAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(0m);
        // Act
        var result = await _exchangeRateService.GetRateAsync("BRL", new DateTime());
        // Assert
        result.Should().NotBeNull();
        result.Rate.Should().Be(0m);
    }

    [Fact]
    public async Task GetRateAsync_With_Negative_Rate_Should_Return_Negative()
    {
        // Arrange
        _exchangeRateClientMock.Setup(s => s.GetExchangeRateAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(-1.25m);
        // Act
        var result = await _exchangeRateService.GetRateAsync("BRL", new DateTime());
        // Assert
        result.Should().NotBeNull();
        result.Rate.Should().Be(-1.25m);
    }  
}
