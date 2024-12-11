namespace Prototype.Payment.Api.Requests;

public class CreateCreditCardRequest
{
    public required string CardholderName { get; set; }
    public required string CardNumber { get; set; }
    public DateTime ExpirationDate { get; set; }
}
