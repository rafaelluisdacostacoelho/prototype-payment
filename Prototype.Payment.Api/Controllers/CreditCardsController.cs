﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Property.Application.Commands.CreditCards;
using Prototype.Payment.Api.Requests;
using Prototype.Payment.Api.Responses;

namespace Prototype.Payment.Api.Controllers;

public class CreditCardsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<CreditCardResponse>> CreateCreditCard([FromBody] CreateCreditCardRequest request)
    {
        var command = new CreateCreditCardCommand
        {
            CardNumber = request.CardNumber,
            CardholderName = request.CardholderName,
            ExpirationDate = request.ExpirationDate
        };

        var creditCard = await _mediator.Send(command);

        var response = new CreditCardResponse
        {
            Id = creditCard.Id,
            CardNumber = creditCard.CardNumber,
            CardholderName = creditCard.CardholderName
        };

        return Ok(response);
    }
}
