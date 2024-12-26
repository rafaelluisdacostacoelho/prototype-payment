using MediatR;
using Prototype.Domain.Models;
using Prototype.Domain.Repositories;
using Prototype.Payment.Application.CrossCutting.Serializables.Responses;

namespace Property.Application.Commands.CreditCards;

public class CreateCreditCardCommand : IRequest<CreatedCreditCardResponse>
{
    public required string CardNumber { get; set; }
    public required string CardHolderName { get; set; }
}

public class CreateCreditCardHandler(ICreditCardsRepository repository) : IRequestHandler<CreateCreditCardCommand, CreatedCreditCardResponse>
{
    private readonly ICreditCardsRepository _repository = repository;

    public async Task<CreatedCreditCardResponse> Handle(CreateCreditCardCommand request, CancellationToken cancellationToken)
    {
        var card = new CreditCard
        {
            Id = Guid.NewGuid().ToString(),
            CardNumber = request.CardNumber,
            CardHolderName = request.CardHolderName
        };

        await _repository.CreateAsync(card);

        return new CreatedCreditCardResponse
        {
            Id = card.Id,
            CardHolderName = card.CardHolderName,
            CardNumber = card.CardNumber
        };
    }
}
