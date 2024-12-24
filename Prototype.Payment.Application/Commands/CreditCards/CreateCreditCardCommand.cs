using MediatR;
using Prototype.Domain.Models;
using Prototype.Domain.Repositories;
using Prototype.Payment.Api.Responses;

namespace Property.Application.Commands.CreditCards;

public class CreateCreditCardCommand : IRequest<CreditCardResponse>
{
    public required string CardNumber { get; set; }
    public required string CardHolderName { get; set; }
}

public class CreateCreditCardHandler(ICreditCardsRepository repository) : IRequestHandler<CreateCreditCardCommand, CreditCardResponse>
{
    private readonly ICreditCardsRepository _repository = repository;

    public async Task<CreditCardResponse> Handle(CreateCreditCardCommand request, CancellationToken cancellationToken)
    {
        var card = new CreditCard
        {
            Id = Guid.NewGuid().ToString(),
            CardNumber = request.CardNumber,
            CardHolderName = request.CardHolderName
        };

        await _repository.CreateAsync(card);

        return new CreditCardResponse
        {
            Id = card.Id,
            CardHolderName = card.CardHolderName,
            CardNumber = card.CardNumber
        };
    }
}
