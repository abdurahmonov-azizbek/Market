using Market.Domain.Entities.Common;

namespace Market.Domain.Entities;

public class ReturnedProduct : EntityBase
{
    public string? Title { get; set; }
    public long Code { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }
}
