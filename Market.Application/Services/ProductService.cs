using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Helpers;

namespace Market.Application.Services;

public class ProductService(AppDbContext dbContext) : IProductService
{
    public async ValueTask<Product> CreateAsync(Guid userId, ProductDTO productDTO)
    {
        if (dbContext.Products.Any(product => product.Code == productDTO.Code))
            throw new InvalidOperationException("Product already exists with this code!");

        var percent = (productDTO.SalePrice - productDTO.IncomingPrice) / (float)productDTO.IncomingPrice * 100;
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = productDTO.Title,
            IncomingPrice = productDTO.IncomingPrice,
            SalePrice = productDTO.SalePrice,
            Percent = (int)percent,
            Count = productDTO.Count,
            Code = productDTO.Code,
            UserId = userId,
            CategoryId = productDTO.CategoryId,
            CreatedDate = Helper.GetCurrentDateTime()
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
        product.DeletedDate = Helper.GetCurrentDateTime();

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

        if (productDTO.Count < 0)
            throw new InvalidOperationException("Count must be positive!");

        product.Title = productDTO.Title;
        product.IncomingPrice = productDTO.IncomingPrice;
        product.SalePrice = productDTO.SalePrice;
        product.Percent = ((productDTO.SalePrice - productDTO.IncomingPrice) / productDTO.IncomingPrice) * 100;
        product.CategoryId = productDTO.CategoryId;
        product.Code = productDTO.Code;
        product.Count = productDTO.Count;
        product.UpdatedDate = Helper.GetCurrentDateTime();

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync();

        return product;
    }
}
