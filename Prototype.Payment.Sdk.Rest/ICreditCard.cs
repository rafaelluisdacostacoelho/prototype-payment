
using Prototype.Payment.Application.CrossCutting.Serializables.Requests;
using Prototype.Payment.Application.CrossCutting.Serializables.Responses;
using RestEase;

namespace Prototype.Payment.Sdk.Rest;

public interface ICreditCard
{
    [Post("credit-cards")]
    Task<CreatedCreditCardResponse> CreateCreditCardAsync([Body] CreateCreditCardRequest request);
}
