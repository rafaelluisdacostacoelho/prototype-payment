namespace Prototype.Payment.Api.Responses;

public class CreditCardResponse
{
    public string? Id { get; set; }
    public string? CardholderName { get; set; }
    public string? CardNumber { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string? Cvv { get; set; }
}
