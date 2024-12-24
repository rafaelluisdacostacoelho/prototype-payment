namespace Prototype.Payment.Api.Responses;

public class CreditCardResponse
{
    public required string Id { get; set; }
    public required string CardHolderName { get; set; }
    public required string CardNumber { get; set; }
}
