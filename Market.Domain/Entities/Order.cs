using Market.Domain.Entities.Common;

namespace Market.Domain.Entities;

public class Order : EntityBase
{
    public Guid ProductId { get; set; }
    public long Code { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public Guid UserId { get; set; }
}
