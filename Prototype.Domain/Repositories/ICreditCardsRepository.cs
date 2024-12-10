using Prototype.Domain.Models;

namespace Prototype.Domain.Repositories;

public interface ICreditCardsRepository
{
    Task<CreditCard?> GetByIdAsync(string id);
    Task<CreditCard> CreateAsync(CreditCard creditCard);
    Task<CreditCard?> UpdateAsync(CreditCard creditCard);
    Task<bool> DeleteAsync(string id);
}
