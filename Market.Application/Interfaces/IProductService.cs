using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface IProductService
{
    ValueTask<Product> CreateAsync(long userId, ProductDTO productDTO);
    ValueTask<IQueryable<Product>> GetAll(long userId);
    ValueTask<Product> GetByIdAsync(long productId);
    ValueTask<Product> UpdateAsync(long productId, ProductDTO productDTO);
    ValueTask<Product> DeleteByIdAsync(long productId);
}
