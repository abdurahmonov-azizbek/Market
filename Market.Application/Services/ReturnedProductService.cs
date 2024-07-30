using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Helpers;
using System.Security.Cryptography.X509Certificates;

namespace Market.Application.Services;

public class ReturnedProductService(AppDbContext dbContext) : IReturnedProductService
{
    public async ValueTask<ReturnedProduct> CreateAsync(Guid userId, ReturnedProductDTO returnedProductDTO)
    {
        var returnedProduct = new ReturnedProduct
        {
            Id = Guid.NewGuid(),
            Title = returnedProductDTO.Title,
            Code = returnedProductDTO.Code,
            CategoryId = returnedProductDTO.CategoryId,
            UserId = userId,
            Count = returnedProductDTO.Count,
            Price = returnedProductDTO.Price,
            CreatedDate = Helper.GetCurrentDateTime(),
        };

        await dbContext.ReturnedProducts.AddAsync(returnedProduct);
        await dbContext.SaveChangesAsync();

        return returnedProduct;
    }

    public async ValueTask<ReturnedProduct> DeleteByIdAsync(Guid returnedProductId)
    {
        var found = await dbContext.ReturnedProducts.FindAsync(returnedProductId)
            ?? throw new EntityNotFoundException(typeof(ReturnedProduct));

        found.IsDeleted = true;
        found.DeletedDate = Helper.GetCurrentDateTime();

        dbContext.ReturnedProducts.Update(found);
        await dbContext.SaveChangesAsync();

        return found;
    }

    public ValueTask<IQueryable<ReturnedProduct>> GetAllAsync(Guid userId)
    {
        return new(dbContext.ReturnedProducts.Where(x => x.UserId == userId));
    }

    public async ValueTask<ReturnedProduct> GetByIdAsync(Guid returnedProductId)
    {
        var found = await dbContext.ReturnedProducts.FindAsync(returnedProductId)
            ?? throw new EntityNotFoundException(typeof(ReturnedProduct));

        return found;
    }

    public async ValueTask<ReturnedProduct> UpdateAsync(Guid returnedProductId, ReturnedProductDTO returnedProductDTO)
    {
        var found = await dbContext.ReturnedProducts.FindAsync(returnedProductId)
            ?? throw new EntityNotFoundException(typeof(ReturnedProduct));

        found.Title = returnedProductDTO.Title;
        found.Code = returnedProductDTO.Code;
        found.CategoryId = returnedProductDTO.CategoryId;
        found.Count = returnedProductDTO.Count;
        found.Price = returnedProductDTO.Price;
        found.UpdatedDate = Helper.GetCurrentDateTime();

        dbContext.ReturnedProducts.Update(found);
        await dbContext.SaveChangesAsync();

        return found;
    }
}
