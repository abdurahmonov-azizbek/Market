using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface ICategoryService
{
    ValueTask<Category> CreateAsync(Guid userId, CategoryDTO categoryDTO);
    ValueTask<IQueryable<Category>> GetAllAsync(Guid userId);
    ValueTask<Category> GetByIdAsync(Guid categoryId);
    ValueTask<Category> UpdateAsync(Guid categoryId, CategoryDTO categoryDTO);
    ValueTask<Category> DeleteByIdAsync(Guid categoryId);
}
