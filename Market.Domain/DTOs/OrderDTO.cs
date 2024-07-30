using Market.Domain.Enums;

namespace Market.Domain.DTOs;

public class OrderDTO
{
    public Guid ProductId { get; set; }
    public long Code { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public PaymentType PaymentType { get; set; }
}
