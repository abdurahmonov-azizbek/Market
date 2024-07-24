using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface IProductService
{
    ValueTask<Product> CreateAsync(Guid userId, ProductDTO productDTO);
    ValueTask<IQueryable<Product>> GetAll(Guid userId);
    ValueTask<Product> GetByIdAsync(Guid productId);
    ValueTask<Product> UpdateAsync(Guid productId, ProductDTO productDTO);
    ValueTask<Product> DeleteByIdAsync(Guid productId);
}
