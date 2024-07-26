namespace Market.Domain.DTOs;

public class ReturnedProductDTO
{
    public string? Title { get; set; }
    public long Code { get; set; }
    public Guid CategoryId { get; set; }
}
