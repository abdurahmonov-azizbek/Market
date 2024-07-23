using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface IProductItemService
{
    ValueTask<ProductItem> CreateAsync(long userId, ProductItemDTO productItemDTO);
    ValueTask<IQueryable<ProductItem>> GetAll(long userId);
    ValueTask<ProductItem> GetByIdAsync(long productItemId);
    ValueTask<ProductItem> UpdateAsync(long productItemId, ProductItemDTO productItemDTO);
    ValueTask<ProductItem> DeleteByIdAsync(long productItemId);
}
