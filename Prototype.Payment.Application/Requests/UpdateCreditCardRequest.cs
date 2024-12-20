﻿namespace Prototype.Payment.Api.Requests;

public class UpdateCreditCardRequest
{
    public required string Id { get; set; }
    public required string CardholderName { get; set; }
    public required string CardNumber { get; set; }
    public DateTime ExpirationDate { get; set; }
    public required string Cvv { get; set; }
}
