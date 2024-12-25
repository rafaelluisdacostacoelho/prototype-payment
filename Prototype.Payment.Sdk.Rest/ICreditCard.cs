using Prototype.Payment.Sdk.Rest.Serializations.Requests;
using Prototype.Payment.Sdk.Rest.Serializations.Responses;
using RestEase;

namespace Prototype.Payment.Sdk.Rest;

public interface ICreditCard
{
    [Post("credit-cards")]
    Task<CreditCardResponse> CreateCreditCard([Body] CreateCreditCardRequest request);
}
