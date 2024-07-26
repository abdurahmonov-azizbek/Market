using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Helpers;
using System.Data;

namespace Market.Application.Services;

public class OrderService(AppDbContext dbContext) : IOrderService
{
    public async ValueTask<Order> CreateAsync(Guid userId, OrderDTO orderDTO)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            ProductItemId = orderDTO.ProductItemId,
            Price = orderDTO.Price,
            UserId = userId,
            CreatedDate = Helper.GetCurrentDateTime()
        };
            
        await dbContext.Orders.AddAsync(order);
        await dbContext.SaveChangesAsync();

        return order;
    }

    public async ValueTask<Order> DeleteByIdAsync(Guid orderId)
    {
        var order = await dbContext.Orders.FindAsync(orderId)
            ?? throw new EntityNotFoundException(typeof(Order));

        order.IsDeleted = true;
        order.DeletedDate = Helper.GetCurrentDateTime();

        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync();

        return order;
    }

    public ValueTask<IQueryable<Order>> GetAll(Guid userId)
        => new(dbContext.Orders.AsQueryable().Where(x => x.UserId == userId));

    public async ValueTask<Order> GetByIdAsync(Guid orderId)
    {
        var order = await dbContext.Orders.FindAsync(orderId)
           ?? throw new EntityNotFoundException(typeof(Order));

        return order;
    }

    public async ValueTask<Order> UpdateAsync(Guid orderId, OrderDTO orderDTO)
    {
        var order = await dbContext.Orders.FindAsync(orderId)
           ?? throw new EntityNotFoundException(typeof(Order));

        order.ProductItemId = orderDTO.ProductItemId;
        order.Price = orderDTO.Price;
        order.UpdatedDate = Helper.GetCurrentDateTime();

        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync();

        return order;
    }
}
