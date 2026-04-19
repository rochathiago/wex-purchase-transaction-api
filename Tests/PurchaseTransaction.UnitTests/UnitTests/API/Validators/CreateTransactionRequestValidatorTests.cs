using PurchaseTransactionAPI.API.DTOs.Requests;
using PurchaseTransactionAPI.API.Validators;

namespace PurchaseTransaction.Tests.UnitTests.API.Validators;

public class CreateTransactionRequestValidatorTests
{

    private readonly CreateTransactionRequestValidator _validator = new();

    [Fact]
    public void Validate_ValidRequest_NoValidationErrors()
    {
        // Arrange
        var request = new CreateTransactionRequest
        {
            Description = "Valid description",
            TransactionDate = DateTime.UtcNow.AddDays(-30),
            Amount = 123.45m
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_DescriptionNullOrEmpty_HasDescriptionError(string description)
    {
        // Arrange
        var request = new CreateTransactionRequest
        {
            Description = description,
            TransactionDate = DateTime.UtcNow,
            Amount = 1.00m
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Description));
    }

    [Fact]
    public void Validate_DescriptionTooLong_HasDescriptionError()
    {
        // Arrange
        var longDescription = new string('A', 51); // max is 50
        var request = new CreateTransactionRequest
        {
            Description = longDescription,
            TransactionDate = DateTime.UtcNow,
            Amount = 1.00m
        };

        // Act  
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Description));
    }

    [Fact]
    public void Validate_TransactionDateInFuture_HasTransactionDateError()
    {
        // Arrange
        var request = new CreateTransactionRequest
        {
            Description = "desc",
            TransactionDate = DateTime.UtcNow.AddMinutes(5),
            Amount = 1.00m
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.TransactionDate));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Validate_AmountLessOrEqualZero_HasAmountError(decimal amount)
    {
        // Arrange
        var request = new CreateTransactionRequest
        {
            Description = "desc",
            TransactionDate = DateTime.UtcNow,
            Amount = amount
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Amount));
    }

    [Fact]
    public void Validate_AmountTooManyDecimalPlaces_HasAmountError()
    {
        // Arrange
        var request = new CreateTransactionRequest
        {
            Description = "desc",
            TransactionDate = DateTime.UtcNow,
            Amount = 1.234m // more than 2 decimal places
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Amount));
    }
}

