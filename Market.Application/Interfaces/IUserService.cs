using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface IUserService
{
    ValueTask<User> CreateAsync(UserDTO userDTO);
    ValueTask<IQueryable<User>> GetAllAsync();
    ValueTask<User> GetByIdAsync(Guid userId);
    ValueTask<User> UpdateAsync(Guid userId, UserDTO userDTO);
    ValueTask<User> DeleteById(Guid userId);
}