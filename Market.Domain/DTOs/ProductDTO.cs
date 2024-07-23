namespace Market.Domain.DTOs;

public class ProductDTO
{
    public string Title { get; set; } = default!;
    public int IncomingPrice { get; set; }
    public int SalePrice { get; set; }
    public long CategoryId { get; set; }
}
