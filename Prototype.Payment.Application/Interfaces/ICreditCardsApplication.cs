using Prototype.Payment.Api.Requests;
using Prototype.Payment.Api.Responses;

namespace Property.Application.Interfaces;

public interface ICreditCardsApplication
{
    Task<CreditCardResponse> CreateAsync(CreateCreditCardRequest request);
    Task<CreditCardResponse> UpdateAsync(UpdateCreditCardRequest request);
    Task<CreditCardResponse> GetAsync(GetCreditCardRequest request);
}
