using Market.Domain.Entities.Common;

namespace Market.Domain.Entities;

public class Order : EntityBase
{
    public Guid ProductItemId { get; set; }
    public decimal Price { get; set; }
    public Guid UserId { get; set; }
}
