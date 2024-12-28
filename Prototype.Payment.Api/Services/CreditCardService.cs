using Grpc.Core;
using MediatR;
using Property.Application.Commands.CreditCards;
using Prototype.Payment.Sdk.Grpc;

namespace Prototype.Payment.Api.Services;

public class CreditCardService(IMediator mediator) : CreditCardGrpcService.CreditCardGrpcServiceBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<CreatedCreditCardResponse> CreateCreditCard(CreateCreditCardRequest request, ServerCallContext context)
    {
        var command = new CreateCreditCardCommand
        {
            CardNumber = request.CardNumber,
            CardHolderName = request.CardHolderName
        };

        var creditCard = await _mediator.Send(command);

        return new CreatedCreditCardResponse
        {
            Id = creditCard.Id,
            CardNumber = creditCard.CardNumber,
            CardHolderName = creditCard.CardHolderName
        };
    }
}
