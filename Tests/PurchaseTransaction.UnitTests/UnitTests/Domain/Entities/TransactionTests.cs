using PurchaseTransactionAPI.Domain.Entities;

namespace PurchaseTransaction.Tests.UnitTests.Domain.Entities;

public class TransactionTests
{
    [Fact]
    public void Constructor_SetsProperties_WhenValidInput()
    {
        // Arrange
        var description = "Valid description";
        var date = new DateTime(2024, 1, 1);
        var amount = 12.346m; // chosen to avoid midpoint rounding ambiguity

        // Act
        var tx = new Transaction(description, date, amount);

        // Assert
        Assert.NotEqual(Guid.Empty, tx.Id);
        Assert.Equal(description, tx.Description);
        Assert.Equal(date, tx.TransactionDate);
        Assert.Equal(12.35m, tx.AmountUsd); // Math.Round(amount, 2)
    }

    [Fact]
    public void Constructor_AllowsDescriptionWithMaxLength_50()
    {
        // Arrange
        var description = new string('a', 50);
        var date = DateTime.UtcNow;
        var amount = 1.23m;

        // Act
        var tx = new Transaction(description, date, amount);

        // Assert
        Assert.Equal(description, tx.Description);
        Assert.Equal(1.23m, tx.AmountUsd);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenDescriptionIsNull()
    {
        // Arrange
        string description = null;
        var date = DateTime.UtcNow;
        var amount = 10m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Transaction(description, date, amount));
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenDescriptionIsWhitespace()
    {
        // Arrange
        var description = "   ";
        var date = DateTime.UtcNow;
        var amount = 10m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Transaction(description, date, amount));
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenDescriptionTooLong()
    {
        // Arrange
        var description = new string('b', 51);
        var date = DateTime.UtcNow;
        var amount = 10m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Transaction(description, date, amount));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-5.0)]
    public void Constructor_ThrowsArgumentException_WhenAmountNotPositive(decimal amount)
    {
        // Arrange
        var description = "desc";
        var date = DateTime.UtcNow;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Transaction(description, date, amount));
    }
}
