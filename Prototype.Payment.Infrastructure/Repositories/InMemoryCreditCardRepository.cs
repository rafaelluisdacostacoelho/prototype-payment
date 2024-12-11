using Prototype.Domain.Models;
using Prototype.Domain.Repositories;

namespace Prototype.Infrastructure.Repositories;

internal class InMemoryCreditCardRepository : ICreditCardsRepository
{
    private readonly List<CreditCard> _creditCards = [];

    public async Task<CreditCard> GetByIdAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(id));

        var creditCard = _creditCards.FirstOrDefault(c => c.Id == id);

        ArgumentNullException.ThrowIfNull(creditCard);

        return await Task.FromResult(creditCard);
    }

    public async Task<CreditCard> CreateAsync(CreditCard creditCard)
    {
        _creditCards.Add(creditCard);

        return await Task.FromResult(creditCard);
    }

    public async Task<CreditCard> UpdateAsync(CreditCard creditCard)
    {
        ArgumentNullException.ThrowIfNull(nameof(creditCard));

        var existingCard = _creditCards.FirstOrDefault(c => c.Id == creditCard.Id);

        ArgumentNullException.ThrowIfNull(existingCard);

        existingCard.CardNumber = creditCard.CardNumber;
        existingCard.CardholderName = creditCard.CardholderName;

        return await Task.FromResult(existingCard);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(id));

        var card = _creditCards.FirstOrDefault(c => c.Id == id);

        if (card != null)
        {
            _creditCards.Remove(card);
            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }
}
