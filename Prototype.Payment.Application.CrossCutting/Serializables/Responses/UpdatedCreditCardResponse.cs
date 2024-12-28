﻿namespace Prototype.Payment.Application.CrossCutting.Serializables.Responses;

public class UpdatedCreditCardResponse
{
    public required string Id { get; set; }
    public required string CardHolderName { get; set; }
    public required string CardNumber { get; set; }
}