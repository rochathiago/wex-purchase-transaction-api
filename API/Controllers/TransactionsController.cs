using Microsoft.AspNetCore.Mvc;
using PurchaseTransactionAPI.API.DTOs.Requests;
using PurchaseTransactionAPI.Application.Interfaces;

namespace PurchaseTransactionAPI.API.Controllers;

/// <summary>
/// REST API controller for managing purchase transactions.
/// </summary>
/// <remarks>
/// This controller provides endpoints to create and retrieve purchase transactions.
/// It supports currency conversion when retrieving transaction details.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class TransactionsController(ITransactionService _transactionService) : ControllerBase
{
    /// <summary>
    /// Creates a new purchase transaction.
    /// </summary>
    /// <param name="request">The transaction creation request containing transaction details.</param>
    /// <returns>
    /// A <see cref="CreatedAtActionResult"/> with HTTP 201 status containing the created transaction.
    /// </returns>
    /// <remarks>
    /// This endpoint creates a new transaction and returns the created resource with its generated ID.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
    {
        var result = await _transactionService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Retrieves a purchase transaction by ID with optional currency conversion.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction to retrieve.</param>
    /// <param name="currencyCode">The target currency code for conversion (e.g., "BRL", "EUR", "SEK"). Optional.</param>
    /// <returns>
    /// An <see cref="OkResult"/> with HTTP 200 status containing the transaction data,
    /// optionally converted to the specified currency.
    /// </returns>
    /// <remarks>
    /// This endpoint retrieves transaction details and converts monetary amounts to the specified
    /// currency if provided. If the transaction is not found or the currency code is invalid,
    /// appropriate error responses are returned.
    /// </remarks>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]    
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] string currencyCode)
    {
        var result = await _transactionService.GetConvertedAsync(id, currencyCode);
        return Ok(result);
    }
}