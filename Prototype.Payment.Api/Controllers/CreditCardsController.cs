using Microsoft.AspNetCore.Mvc;
using Property.Application.Interfaces;

namespace Prototype.Payment.Api.Controllers;

public class CreditCardsController(ICreditCardsApplication creditCardsApplication) : ControllerBase
{
    private readonly ICreditCardsApplication _creditCardsApplication = creditCardsApplication;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var message = await _creditCardsApplication.Get(name);
        return Ok(message);
    }
}
