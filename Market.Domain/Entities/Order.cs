using Market.Domain.Entities.Common;
using Market.Domain.Enums;

namespace Market.Domain.Entities;

public class Order : EntityBase
{
    public Guid ProductId { get; set; }
    public long Code { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public PaymentType PaymentType { get; set; }
    public Guid UserId { get; set; }
}
