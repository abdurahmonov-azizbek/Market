using Market.Domain.Entities.Common;

namespace Market.Domain.Entities;

public class ProductItem : EntityBase
{
    public long ProductId { get; set; }
    public long Code { get; set; }
    public long UserId { get; set; }
}
