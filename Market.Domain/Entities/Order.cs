using Market.Domain.Entities.Common;

namespace Market.Domain.Entities;

public class Order : EntityBase
{
    public long ProductItemId { get; set; }
    public decimal Price { get; set; }
    public long UserId { get; set; }
}
