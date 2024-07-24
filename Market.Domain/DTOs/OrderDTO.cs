namespace Market.Domain.DTOs;

public class OrderDTO
{
    public Guid ProductItemId { get; set; }
    public decimal Price { get; set; }
}
