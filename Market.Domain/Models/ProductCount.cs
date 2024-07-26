using Market.Domain.Entities;

namespace Market.Domain.Models;

public class ProductCount
{
    public Product Product { get; set; }
    public int Count { get; set; }
}
