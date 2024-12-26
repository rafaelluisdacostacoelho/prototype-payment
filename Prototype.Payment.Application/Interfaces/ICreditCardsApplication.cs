using Prototype.Payment.Application.CrossCutting.Serializables.Requests;
using Prototype.Payment.Application.CrossCutting.Serializables.Responses;

namespace Property.Application.Interfaces;

public interface ICreditCardsApplication
{
    Task<CreatedCreditCardResponse> CreateAsync(CreateCreditCardRequest request);
    Task<UpdatedCreditCardResponse> UpdateAsync(UpdateCreditCardRequest request);
    Task<FoundCreditCardResponse> FindAsync(GetCreditCardRequest request);
}
