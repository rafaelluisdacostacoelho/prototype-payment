using Property.Application.Interfaces;
using Prototype.Domain.Repositories;
using Prototype.Payment.Api.Requests;
using Prototype.Payment.Api.Responses;

namespace Prototype.Payment.Application.Services
{
    public class CreditCardsApplication(ICreditCardsRepository creditCardsRepository) : ICreditCardsApplication
    {
        // Simulação de um banco de dados
        private readonly List<CreditCardResponse> _cards = [];

        public Task<CreditCardResponse> AddCard(AddCreditCardRequest request)
        {
            var card = new CreditCardResponse
            {
                Id = Guid.NewGuid().ToString(),
                CardholderName = request.CardholderName,
                CardNumber = request.CardNumber,
                ExpirationDate = request.ExpirationDate,
                Cvv = request.Cvv
            };
            _cards.Add(card);
            return Task.FromResult(card);
        }

        public Task<CreditCardResponse?> UpdateCard(UpdateCreditCardRequest request)
        {
            var card = _cards.FirstOrDefault(c => c.Id == request.Id);
            if (card != null)
            {
                card.CardholderName = request.CardholderName;
                card.CardNumber = request.CardNumber;
                card.ExpirationDate = request.ExpirationDate;
                card.Cvv = request.Cvv;
            }
            return Task.FromResult(card);
        }

        public Task<CreditCardResponse?> GetCard(GetCreditCardRequest request)
        {
            var card = _cards.FirstOrDefault(c => c.Id == request.Id);
            return Task.FromResult(card);
        }

        public Task<CreditCardsListResponse> ListCards(ListCreditCardsRequest request)
        {
            var response = new CreditCardsListResponse();
            response.Cards.AddRange(_cards);
            return Task.FromResult(response);
        }
    }
}
