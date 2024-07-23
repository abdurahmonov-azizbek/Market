using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using System.Data;

namespace Market.Application.Services;

public class OrderService(AppDbContext dbContext) : IOrderService
{
    public async ValueTask<Order> CreateAsync(long userId, OrderDTO orderDTO)
    {
        var order = new Order
        {
            Id = dbContext.Orders.Count() + 1,
            ProductItemId = orderDTO.ProductItemId,
            Price = orderDTO.Price,
            UserId = userId,
            CreatedDate = DateTime.Now
        };

        await dbContext.Orders.AddAsync(order);
        await dbContext.SaveChangesAsync();

        return order;
    }

    public async ValueTask<Order> DeleteByIdAsync(long orderId)
    {
        var order = await dbContext.Orders.FindAsync(orderId)
            ?? throw new EntityNotFoundException(typeof(Order));

        order.IsDeleted = true;
        order.DeletedDate = DateTime.Now;

        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync();

        return order;
    }

    public ValueTask<IQueryable<Order>> GetAll(long userId)
        => new(dbContext.Orders.AsQueryable().Where(x => x.UserId == userId));

    public async ValueTask<Order> GetByIdAsync(long orderId)
    {
        var order = await dbContext.Orders.FindAsync(orderId)
           ?? throw new EntityNotFoundException(typeof(Order));

        return order;
    }

    public async ValueTask<Order> UpdateAsync(long orderId, OrderDTO orderDTO)
    {
        var order = await dbContext.Orders.FindAsync(orderId)
           ?? throw new EntityNotFoundException(typeof(Order));

        order.ProductItemId = orderDTO.ProductItemId;
        order.Price = orderDTO.Price;
        order.UpdatedDate = DateTime.Now;

        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync();

        return order;
    }
}
