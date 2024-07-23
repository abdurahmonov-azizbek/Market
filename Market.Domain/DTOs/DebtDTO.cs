using Market.Domain.Enums;

namespace Market.Domain.DTOs;

public class DebtDTO
{
    public string Title { get; set; } = default!;
    public DebtType Type { get; set; } = DebtType.Client;
    public decimal Price { get; set; }
}
