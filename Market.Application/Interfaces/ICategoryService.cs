using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface ICategoryService
{
    ValueTask<Category> CreateAsync(long userId, CategoryDTO categoryDTO);
    ValueTask<IQueryable<Category>> GetAllAsync(long userId);
    ValueTask<Category> GetByIdAsync(long categoryId);
    ValueTask<Category> UpdateAsync(long categoryId, CategoryDTO categoryDTO);
    ValueTask<Category> DeleteByIdAsync(long categoryId);
}
