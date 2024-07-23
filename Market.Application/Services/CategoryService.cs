﻿using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;

namespace Market.Application.Services;

public class CategoryService(AppDbContext dbContext) : ICategoryService
{
    public async ValueTask<Category> CreateAsync(long userId, CategoryDTO categoryDTO)
    {
        var category = new Category
        {
            Id = dbContext.Categories.Count() + 1,
            Title = categoryDTO.Title,
            UserId = userId,
            CreatedDate = DateTime.Now,
        };

        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();

        return category;
    }

    public async ValueTask<Category> DeleteByIdAsync(long categoryId)
    {
        var category = await dbContext.Categories.FindAsync(categoryId)
            ?? throw new EntityNotFoundException(typeof(Category));

        category.IsDeleted = true;
        category.DeletedDate = DateTime.Now;

        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync();

        return category;
    }

    public ValueTask<IQueryable<Category>> GetAllAsync(long userId)
        => new(dbContext.Categories.AsQueryable().Where(category => category.UserId == userId));

    public async ValueTask<Category> GetByIdAsync(long categoryId)
    {
        var category = await dbContext.Categories.FindAsync(categoryId)
            ?? throw new EntityNotFoundException(typeof(Category));

        return category;
    }

    public async ValueTask<Category> UpdateAsync(long categoryId, CategoryDTO categoryDTO)
    {
        var category = await dbContext.Categories.FindAsync(categoryId)
            ?? throw new EntityNotFoundException(typeof(Category));

        category.Title = categoryDTO.Title;
        category.UpdatedDate = DateTime.Now;

        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync();

        return category;
    }
}