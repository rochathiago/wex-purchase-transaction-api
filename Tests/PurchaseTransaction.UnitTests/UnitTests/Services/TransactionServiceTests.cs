using FluentAssertions;
using Moq;
using PurchaseTransactionAPI.API.DTOs.Requests;
using PurchaseTransactionAPI.Application.Interfaces;
using PurchaseTransactionAPI.Application.Services;
using PurchaseTransactionAPI.Domain.Entities;

namespace PurchaseTransaction.Tests.UnitTests.Services;

public class TransactionServiceTests
{
    private readonly Mock<IExchangeRateService> _exchangeRateMock;
    private readonly Mock<ITransactionRepository> _transactionRepository;
    private readonly TransactionService _service;

    public TransactionServiceTests()
    {
        _exchangeRateMock = new Mock<IExchangeRateService>();
        _transactionRepository = new Mock<ITransactionRepository>();
        _service = new TransactionService(_transactionRepository.Object, _exchangeRateMock.Object);
    }

    [Fact]
    public async Task CreateAsync_With_Valid_Request_Should_Return_Valid_TransactionResponse()
    {
        // Arrange
        var request = new CreateTransactionRequest
        {
            Description = "Test Transaction",
            TransactionDate = DateTime.UtcNow,
            Amount = 100m
        };

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Description.Should().Be(request.Description);
        result.TransactionDate.Should().Be(request.TransactionDate);
        result.AmountUsd.Should().Be(request.Amount);
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(0)]
    public async Task CreateAsync_With_Invalid_Amount_Should_Throw_ArgumentException(decimal amount)
    {
        // Arrange
        var request = new CreateTransactionRequest
        {
            Description = "Test Transaction",
            TransactionDate = DateTime.UtcNow,
            Amount = amount
        };

        // Act
        Func<Task> act = async () => { await _service.CreateAsync(request); };

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Amount must be positive");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateAsync_With_Null_Or_Empty_Description_Should_Throw_ArgumentException(string description)
    {
        // Arrange
        var request = new CreateTransactionRequest
        {
            Description = description,
            TransactionDate = DateTime.UtcNow,
            Amount = 100m
        };

        // Act
        Func<Task> act = async () => { await _service.CreateAsync(request); };

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid description");
    }


    [Fact]
    public async Task CreateAsync_With_Description_Exceeding_Max_Length_Should_Throw_ArgumentException()
    {
        // Arrange
        var request = new CreateTransactionRequest
        {
            Description = new string('A', 51), // 51 characters
            TransactionDate = DateTime.UtcNow,
            Amount = 100m
        };
        // Act
        Func<Task> act = async () => { await _service.CreateAsync(request); };
        
        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid description");
    }

    [Fact]
    public async Task GetConvertedAsync_With_Nonexistent_Transaction_Should_Throw_KeyNotFoundException()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        _transactionRepository.Setup(r => r.GetByIdAsync(transactionId))
            .ReturnsAsync(null as Transaction);
        
        // Act
        Func<Task> act = async () => { await _service.GetConvertedAsync(transactionId, "EUR"); };
        
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Transaction not found");
    }
}
