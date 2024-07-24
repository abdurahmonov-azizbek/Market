using Market.Domain.Entities.Common;

namespace Market.Domain.Entities;

public class ProductItem : EntityBase
{
    public string? Title { get; set; }
    public Guid ProductId { get; set; }
    public long Code { get; set; }
    public Guid UserId { get; set; }
}
