using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;

namespace Market.Application.Services;

public class ProductItemService(AppDbContext dbContext) : IProductItemService
{
    public async ValueTask<ProductItem> CreateAsync(long userId, ProductItemDTO productItemDTO)
    {
        var productItem = new ProductItem
        {
            Id = dbContext.ProductItems.Count() + 1,
            ProductId = productItemDTO.ProductId,
            Code = productItemDTO.Code,
            UserId = userId,
            CreatedDate = DateTime.Now
        };

        await dbContext.ProductItems.AddAsync(productItem);
        await dbContext.SaveChangesAsync();

        return productItem;
    }

    public async ValueTask<ProductItem> DeleteByIdAsync(long productItemId)
    {
        var productItem = await dbContext.ProductItems.FindAsync(productItemId)
            ?? throw new EntityNotFoundException(typeof(ProductItem));

        productItem.IsDeleted = true;
        productItem.DeletedDate = DateTime.Now;

        dbContext.ProductItems.Update(productItem);
        await dbContext.SaveChangesAsync();

        return productItem;
    }

    public ValueTask<IQueryable<ProductItem>> GetAll(long userId)
         => new(dbContext.ProductItems.AsQueryable().Where(x => x.UserId == userId));

    public async ValueTask<ProductItem> GetByIdAsync(long productItemId)
    {
        var productItem = await dbContext.ProductItems.FindAsync(productItemId)
            ?? throw new EntityNotFoundException(typeof(ProductItem));

        return productItem;
    }

    public async ValueTask<ProductItem> UpdateAsync(long productItemId, ProductItemDTO productItemDTO)
    {
        var productItem = await dbContext.ProductItems.FindAsync(productItemId)
            ?? throw new EntityNotFoundException(typeof(ProductItem));

        productItem.ProductId = productItemDTO.ProductId;
        productItem.Code = productItemDTO.Code;
        productItem.UpdatedDate = DateTime.Now;

        dbContext.ProductItems.Update(productItem);
        await dbContext.SaveChangesAsync();

        return productItem;
    }
}
