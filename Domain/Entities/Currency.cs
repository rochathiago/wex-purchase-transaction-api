namespace PurchaseTransactionAPI.Domain.Entities;

public class Currency
{
    public string Code { get; private set; }
    public string Description { get; private set; }

    public Currency() { }
    public Currency(string code, string description)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code is required");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required");

        Code = code.ToUpper();
        Description = description;
    }
}
