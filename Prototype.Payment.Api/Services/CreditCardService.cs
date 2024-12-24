using Grpc.Core;
using MediatR;
using Property.Application.Commands.CreditCards;

namespace Prototype.Payment.Api.Services;

public class CreditCardService(IMediator mediator) : CreditCard.CreditCardService.CreditCardServiceBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<CreditCard.CreditCardResponse> CreateCreditCard(CreditCard.CreateCreditCardRequest request, ServerCallContext context)
    {
        var command = new CreateCreditCardCommand
        {
            CardNumber = request.CardNumber,
            CardHolderName = request.CardHolderName
        };

        var creditCard = await _mediator.Send(command);

        return new CreditCard.CreditCardResponse
        {
            Id = creditCard.Id,
            CardNumber = creditCard.CardNumber,
            CardHolderName = creditCard.CardHolderName
        };
    }
}
