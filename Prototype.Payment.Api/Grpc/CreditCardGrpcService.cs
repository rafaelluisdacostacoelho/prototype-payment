using Grpc.Core;
using MediatR;
using Property.Application.Commands.CreditCards;
using Prototype.Payment.Api.Requests;
using Prototype.Payment.Api.Responses;

namespace Prototype.Payment.Api.Procedures;

public class CreditCardGrpcService(IMediator mediator)
{
    private readonly IMediator _mediator = mediator;

    public async Task<CreditCardResponse> CreateCreditCard(CreateCreditCardRequest request, ServerCallContext context)
    {
        var command = new CreateCreditCardCommand
        {
            CardNumber = request.CardNumber,
            CardholderName = request.CardholderName,
            ExpirationDate = request.ExpirationDate
        };

        var creditCard = await _mediator.Send(command);

        return new CreditCardResponse
        {
            Id = creditCard.Id,
            CardNumber = creditCard.CardNumber,
            CardholderName = creditCard.CardholderName
        };
    }
}
