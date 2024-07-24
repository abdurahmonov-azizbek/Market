using Market.Domain.Entities.Common;

namespace Market.Domain.Entities;

public class ProductItem : EntityBase
{
    public Guid ProductId { get; set; }
    public long Code { get; set; }
    public Guid UserId { get; set; }
}
