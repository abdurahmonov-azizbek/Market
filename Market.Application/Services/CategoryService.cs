using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Helpers;

namespace Market.Application.Services;

public class CategoryService(AppDbContext dbContext) : ICategoryService
{
    public async ValueTask<Category> CreateAsync(Guid userId, CategoryDTO categoryDTO)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Title = categoryDTO.Title,
            UserId = userId,
            CreatedDate = Helper.GetCurrentDateTime(),
        };

        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();

        return category;
    }

    public async ValueTask<Category> DeleteByIdAsync(Guid categoryId)
    {
        var category = await dbContext.Categories.FindAsync(categoryId)
            ?? throw new EntityNotFoundException(typeof(Category));

        category.IsDeleted = true;
        category.DeletedDate = Helper.GetCurrentDateTime();

        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync();

        return category;
    }

    public ValueTask<IQueryable<Category>> GetAllAsync(Guid userId)
        => new(dbContext.Categories.AsQueryable().Where(category => category.UserId == userId));

    public async ValueTask<Category> GetByIdAsync(Guid categoryId)
    {
        var category = await dbContext.Categories.FindAsync(categoryId)
            ?? throw new EntityNotFoundException(typeof(Category));

        return category;
    }

    public async ValueTask<Category> UpdateAsync(Guid categoryId, CategoryDTO categoryDTO)
    {
        var category = await dbContext.Categories.FindAsync(categoryId)
            ?? throw new EntityNotFoundException(typeof(Category));

        category.Title = categoryDTO.Title;
        category.UpdatedDate = Helper.GetCurrentDateTime();

        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync();

        return category;
    }
}
