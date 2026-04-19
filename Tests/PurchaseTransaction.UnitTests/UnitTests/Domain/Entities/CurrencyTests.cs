using System;
using Xunit;
using PurchaseTransactionAPI.Domain.Entities;

namespace PurchaseTransaction.Tests.UnitTests.Domain.Entities;

public class CurrencyTests
{
    [Fact]
    public void Constructor_ValidInputs_SetsProperties()
    {
        // Arrange
        var code = "usd";
        var description = "US Dollar";

        // Act
        var currency = new Currency(code, description);

        // Assert
        Assert.Equal("USD", currency.Code); // Code should be stored uppercased
        Assert.Equal(description, currency.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidCode_ThrowsArgumentException(string invalidCode)
    {
        // Arrange
        var description = "Some description";

        // Act
        var ex = Assert.Throws<ArgumentException>(() => new Currency(invalidCode, description));

        // Assert
        Assert.Equal("Code is required", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidDescription_ThrowsArgumentException(string invalidDescription)
    {
        // Arrange
        var code = "EUR";

        // Act
        var ex = Assert.Throws<ArgumentException>(() => new Currency(code, invalidDescription));

        // Assert
        Assert.Equal("Description is required", ex.Message);
    }

    [Fact]
    public void ParameterlessConstructor_AllowsCreation_WithDefaultValues()
    {
        // Arrange & Act
        var currency = new Currency();

        // Assert
        Assert.Null(currency.Code);
        Assert.Null(currency.Description);
    }
}


