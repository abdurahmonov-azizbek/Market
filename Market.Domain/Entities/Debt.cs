using Market.Domain.Entities.Common;
using Market.Domain.Enums;

namespace Market.Domain.Entities;

public class Debt : EntityBase
{
    public string Title { get; set; } = default!;
    public DebtType Type { get; set; } = DebtType.Client;
    public decimal Price { get; set; }
    public long UserId { get; set; }
}
