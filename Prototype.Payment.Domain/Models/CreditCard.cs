namespace Prototype.Domain.Models;

public class CreditCard
{
    public required string Id { get; set; }
    public required string CardNumber { get; set; }
    public required string CardholderName { get; set; }
    public DateTime ExpirationDate { get; set; }
}
