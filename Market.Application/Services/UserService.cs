using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;

namespace Market.Application.Services;

public class UserService(AppDbContext dbContext) : IUserService
{
    public async ValueTask<User> CreateAsync(UserDTO userDTO)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            Email = userDTO.Email,
            Role = userDTO.Role,
            Password = userDTO.Password,
            CreatedDate = DateTime.Now,
        };

        var entityEntry = await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async ValueTask<User> DeleteById(Guid userId)
    {
        var user = await dbContext.Users.FindAsync(userId)
            ?? throw new EntityNotFoundException(typeof(User));

        user.IsDeleted = true;
        user.DeletedDate = DateTime.Now;
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();

        return user;
    }

    public ValueTask<IQueryable<User>> GetAllAsync()
        => new(dbContext.Users.AsQueryable());

    public async ValueTask<User> GetByIdAsync(Guid userId)
    {
        var user = await dbContext.Users.FindAsync(userId)
            ?? throw new EntityNotFoundException(typeof(User));

        return user;
    }

    public async ValueTask<User> UpdateAsync(Guid userId, UserDTO userDTO)
    {
        var user = await dbContext.Users.FindAsync(userId)
           ?? throw new EntityNotFoundException(typeof(User));

        user.FirstName = userDTO.FirstName;
        user.LastName = userDTO.LastName;
        user.Email = userDTO.Email;
        user.Password = userDTO.Password;
        user.Role = userDTO.Role;
        user.UpdatedDate = DateTime.Now;

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();

        return user;
    }
}
