using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface IReturnedProductService
{
    ValueTask<ReturnedProduct> CreateAsync(Guid userId, ReturnedProductDTO returnedProductDTO);
    ValueTask<IQueryable<ReturnedProduct>> GetAllAsync(Guid userId);
    ValueTask<ReturnedProduct> GetByIdAsync(Guid returnedProductId);
    ValueTask<ReturnedProduct> UpdateAsync(Guid returnedProductId, ReturnedProductDTO returnedProductDTO);
    ValueTask<ReturnedProduct> DeleteByIdAsync(Guid returnedProductId);
}
