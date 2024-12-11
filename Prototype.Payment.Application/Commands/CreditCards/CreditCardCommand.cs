using MediatR;
using Prototype.Domain.Models;
using Prototype.Domain.Repositories;

namespace Property.Application.Commands.CreditCards;

public class CreateCreditCardCommand : IRequest<CreditCard>
{
    public string? Number { get; set; }
    public string? Holder { get; set; }
}

public class CreateCreditCardHandler : IRequestHandler<CreateCreditCardCommand, CreditCard>
{
    private readonly ICreditCardsRepository _repository;

    public CreateCreditCardHandler(ICreditCardsRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreditCard> Handle(CreateCreditCardCommand request, CancellationToken cancellationToken)
    {
        var creditCard = new CreditCard { Number = request.Number, Holder = request.Holder };
        return await _repository.CreateAsync(creditCard);
    }
}
