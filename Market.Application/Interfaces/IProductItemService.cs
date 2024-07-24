using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface IProductItemService
{
    ValueTask<ProductItem> CreateAsync(Guid userId, ProductItemDTO productItemDTO);
    ValueTask<IQueryable<ProductItem>> GetAll(Guid userId);
    ValueTask<ProductItem> GetByIdAsync(Guid productItemId);
    ValueTask<ProductItem> UpdateAsync(Guid productItemId, ProductItemDTO productItemDTO);
    ValueTask<ProductItem> DeleteByIdAsync(Guid productItemId);
}
