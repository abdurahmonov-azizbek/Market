using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Helpers;

namespace Market.Application.Services;

public class ProductItemService(AppDbContext dbContext) : IProductItemService
{
    public async ValueTask<ProductItem> CreateAsync(Guid userId, ProductItemDTO productItemDTO)
    {
        var productItem = new ProductItem
        {
            Id = Guid.NewGuid(),
            Title = productItemDTO.Title,
            ProductId = productItemDTO.ProductId,
            Code = productItemDTO.Code,
            UserId = userId,
            CreatedDate = Helper.GetCurrentDateTime()
        };

        await dbContext.ProductItems.AddAsync(productItem);
        await dbContext.SaveChangesAsync();

        return productItem;
    }

    public async ValueTask<ProductItem> DeleteByIdAsync(Guid productItemId)
    {
        var productItem = await dbContext.ProductItems.FindAsync(productItemId)
            ?? throw new EntityNotFoundException(typeof(ProductItem));

        productItem.IsDeleted = true;
        productItem.DeletedDate = Helper.GetCurrentDateTime();

        dbContext.ProductItems.Update(productItem);
        await dbContext.SaveChangesAsync();

        return productItem;
    }

    public ValueTask<IQueryable<ProductItem>> GetAll(Guid userId)
         => new(dbContext.ProductItems.AsQueryable().Where(x => x.UserId == userId));

    public async ValueTask<ProductItem> GetByIdAsync(Guid productItemId)
    {
        var productItem = await dbContext.ProductItems.FindAsync(productItemId)
            ?? throw new EntityNotFoundException(typeof(ProductItem));

        return productItem;
    }

    public async ValueTask<ProductItem> UpdateAsync(Guid productItemId, ProductItemDTO productItemDTO)
    {
        var productItem = await dbContext.ProductItems.FindAsync(productItemId)
            ?? throw new EntityNotFoundException(typeof(ProductItem));

        productItem.ProductId = productItemDTO.ProductId;
        productItem.Code = productItemDTO.Code;
        productItem.Title = productItemDTO.Title;
        productItem.UpdatedDate = Helper.GetCurrentDateTime();

        dbContext.ProductItems.Update(productItem);
        await dbContext.SaveChangesAsync();

        return productItem;
    }
}
