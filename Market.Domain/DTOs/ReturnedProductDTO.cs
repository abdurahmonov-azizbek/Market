namespace Market.Domain.DTOs;

public class ReturnedProductDTO
{
    public string? Title { get; set; }
    public long Code { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
}
