using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;

namespace Market.Application.Services;

public class ProductService(AppDbContext dbContext) : IProductService
{
    public async ValueTask<Product> CreateAsync(Guid userId, ProductDTO productDTO)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = productDTO.Title,
            IncomingPrice = productDTO.IncomingPrice,
            SalePrice = productDTO.SalePrice,
            Percent = ((productDTO.SalePrice - productDTO.IncomingPrice) / productDTO.IncomingPrice) * 100,
            UserId = userId,
            CategoryId = productDTO.CategoryId,
            CreatedDate = DateTime.Now
        };

        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        return product;
    }

    public async ValueTask<Product> DeleteByIdAsync(Guid productId)
    {
        var product = await dbContext.Products.FindAsync(productId)
            ?? throw new EntityNotFoundException(typeof(Product));

        product.IsDeleted = true;
        product.DeletedDate = DateTime.Now;

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync();

        return product;
    }

    public ValueTask<IQueryable<Product>> GetAll(Guid userId)
        => new(dbContext.Products.AsQueryable().Where(x => x.UserId == userId));

    public async ValueTask<Product> GetByIdAsync(Guid productId)
    {
        var product = await dbContext.Products.FindAsync(productId)
           ?? throw new EntityNotFoundException(typeof(Product));

        return product;
    }

    public async ValueTask<Product> UpdateAsync(Guid productId, ProductDTO productDTO)
    {
        var product = await dbContext.Products.FindAsync(productId)
           ?? throw new EntityNotFoundException(typeof(Product));

        product.Title = productDTO.Title;
        product.IncomingPrice = productDTO.IncomingPrice;
        product.SalePrice = productDTO.SalePrice;
        product.Percent = ((productDTO.SalePrice - productDTO.IncomingPrice) / productDTO.IncomingPrice) * 100;
        product.CategoryId = productDTO.CategoryId;
        product.UpdatedDate = DateTime.Now;

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync();

        return product;
    }
}
