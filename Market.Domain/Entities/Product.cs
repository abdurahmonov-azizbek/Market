using Market.Domain.Entities.Common;

namespace Market.Domain.Entities;

public class Product : EntityBase
{
    public string Title { get; set; } = default!;
    public int IncomingPrice { get; set; }
    public int SalePrice { get; set; }
    public int Percent { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
}
