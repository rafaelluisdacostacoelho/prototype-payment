namespace Prototype.Payment.Api.Requests;

public class AddCreditCardRequest
{
    public string? CardholderName { get; set; }
    public string? CardNumber { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string? Cvv { get; set; }
}
