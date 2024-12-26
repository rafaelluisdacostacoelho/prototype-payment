using MediatR;
using Microsoft.AspNetCore.Mvc;
using Property.Application.Commands.CreditCards;
using Prototype.Payment.Application.CrossCutting.Serializables.Requests;
using Prototype.Payment.Application.CrossCutting.Serializables.Responses;

namespace Prototype.Payment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditCardsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<CreatedCreditCardResponse>> CreateCreditCard([FromBody] CreateCreditCardRequest request)
    {
        var command = new CreateCreditCardCommand
        {
            CardNumber = request.CardNumber,
            CardHolderName = request.CardHolderName
        };

        var creditCard = await _mediator.Send(command);

        var response = new CreatedCreditCardResponse
        {
            Id = creditCard.Id,
            CardNumber = creditCard.CardNumber,
            CardHolderName = creditCard.CardHolderName
        };

        return Ok(response);
    }
}
