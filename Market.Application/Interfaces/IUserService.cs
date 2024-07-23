using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface IUserService
{
    ValueTask<User> CreateAsync(UserDTO userDTO);
    ValueTask<IQueryable<User>> GetAllAsync();
    ValueTask<User> GetByIdAsync(long userId);
    ValueTask<User> UpdateAsync(long userId, UserDTO userDTO);
    ValueTask<User> DeleteById(long userId);
}