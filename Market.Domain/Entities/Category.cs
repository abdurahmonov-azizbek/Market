using Market.Domain.Entities.Common;

namespace Market.Domain.Entities;

public class Category : EntityBase
{
    public string Title { get; set; } = default!;
    public Guid UserId { get; set; }
}
