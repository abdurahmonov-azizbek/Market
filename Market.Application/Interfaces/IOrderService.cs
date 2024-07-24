using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface IOrderService
{
    ValueTask<Order> CreateAsync(Guid userId, OrderDTO orderDTO);
    ValueTask<IQueryable<Order>> GetAll(Guid userId);
    ValueTask<Order> GetByIdAsync(Guid orderId);
    ValueTask<Order> UpdateAsync(Guid orderId, OrderDTO orderDTO);
    ValueTask<Order> DeleteByIdAsync(Guid orderId);
}
