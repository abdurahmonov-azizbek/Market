namespace Market.Domain.DTOs;

public class ProductItemDTO
{
    public string? Title { get; set; }
    public Guid ProductId { get; set; }
    public long Code { get; set; }
}
