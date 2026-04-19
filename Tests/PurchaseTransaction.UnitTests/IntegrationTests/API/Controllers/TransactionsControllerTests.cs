using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PurchaseTransactionAPI.API.DTOs.Responses;
using System.Net;
using System.Net.Http.Json;

namespace PurchaseTransaction.Tests.IntegrationTests.API.Controllers;

public class TransactionsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TransactionsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task With_Valid_Request_Should_Create_Transaction_And_Return_Created_StatusCode()
    {
        // Arrange
        var createRequest = new
        {
            description = "Purchase Test",
            transactionDate = DateTime.UtcNow,
            amount = 100
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactions", createRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task With_Valid_Request_Should_Get_Transaction_By_Id_And_Return_OK_StatusCode()
    {
        // Arrange
        var currencyCode = "BRL";
        var createRequest = new
        {
            description = "Purchase Test",
            transactionDate = DateTime.UtcNow,
            amount = 100
        };
        var createResponse = await _client.PostAsJsonAsync("/api/transactions", createRequest);
        var createdTransaction = await createResponse.Content.ReadFromJsonAsync<TransactionResponse>();        
        // Act
        var response = await _client.GetAsync($"/api/transactions/{createdTransaction.Id}?currencyCode={currencyCode}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
