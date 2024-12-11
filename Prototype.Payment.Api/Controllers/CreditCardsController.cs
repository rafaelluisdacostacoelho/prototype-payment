using Microsoft.AspNetCore.Mvc;
using Property.Application.Interfaces;
using Prototype.Payment.Api.Requests;

namespace Prototype.Payment.Api.Controllers;

public class CreditCardsController(ICreditCardsApplication creditCardsApplication) : ControllerBase
{
    private readonly ICreditCardsApplication _creditCardsApplication = creditCardsApplication;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var message = await _creditCardsApplication.GetCard(new GetCreditCardRequest { Id = id});

        return Ok(message);
    }
}
