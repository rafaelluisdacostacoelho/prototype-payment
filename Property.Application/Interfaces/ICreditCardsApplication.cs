using Prototype.Payment.Api.Requests;
using Prototype.Payment.Api.Responses;

namespace Property.Application.Interfaces;

public interface ICreditCardsApplication
{
    Task<CreditCardResponse> AddCard(AddCreditCardRequest request);
    Task<CreditCardResponse?> UpdateCard(UpdateCreditCardRequest request);
    Task<CreditCardResponse?> GetCard(GetCreditCardRequest request);
    Task<CreditCardsListResponse> ListCards(ListCreditCardsRequest request);
}
