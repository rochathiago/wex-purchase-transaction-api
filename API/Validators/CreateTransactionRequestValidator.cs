using FluentValidation;
using PurchaseTransactionAPI.API.DTOs.Requests;

namespace PurchaseTransactionAPI.API.Validators;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .MaximumLength(50);

        RuleFor(x => x.TransactionDate)
            .LessThanOrEqualTo(DateTime.UtcNow);

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .PrecisionScale(18, 2, true);
    }
}