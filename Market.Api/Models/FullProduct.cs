using Market.Domain.Entities;

namespace Market.Api.Models
{
    public class FullProduct
    {
        public Product? Product { get; set; }
        public ProductItem? ProductItem { get; set; }
    }
}
