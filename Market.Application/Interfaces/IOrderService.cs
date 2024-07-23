using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface IOrderService
{
    ValueTask<Order> CreateAsync(long userId, OrderDTO orderDTO);
    ValueTask<IQueryable<Order>> GetAll(long userId);
    ValueTask<Order> GetByIdAsync(long orderId);
    ValueTask<Order> UpdateAsync(long orderId, OrderDTO orderDTO);
    ValueTask<Order> DeleteByIdAsync(long orderId);
}
