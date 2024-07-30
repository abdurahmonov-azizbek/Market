using Market.Domain.Entities;

namespace Market.Domain.Models;

public class OrderStatistics
{
    public int Count { get; set; }
    public Total Total { get; set; }
    public List<ProductCount> Products { get; set; }
}
