using FluentValidation;

namespace PurchaseTransactionAPI.API.Validators;

public class CurrencyValidator : AbstractValidator<string>
{
    public CurrencyValidator()
    {
        //RuleFor(x => x)
        //    .NotEmpty()
        //    .Length(3)
        //    .Matches("^[A-Z]{3}$");
        RuleFor(x => x)
            .NotEmpty();
    }
}