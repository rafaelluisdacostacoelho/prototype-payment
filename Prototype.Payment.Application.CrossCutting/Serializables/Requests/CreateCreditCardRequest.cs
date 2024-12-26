namespace Prototype.Payment.Application.CrossCutting.Serializables.Requests;

public class CreateCreditCardRequest
{
    public required string CardHolderName { get; set; }
    public required string CardNumber { get; set; }
}
