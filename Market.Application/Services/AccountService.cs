using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Models;

namespace Market.Application.Services
{
    public class AccountService(
        IUserService userService,
        IPasswordHasherService passwordHasherService) : IAccountService
    {
        public async ValueTask<bool> GrandRoleAsync(GrandRoleDetails grandRoleDetails)
        {
            var fromUser = await userService.GetByIdAsync(grandRoleDetails.FromUserId)
                ?? throw new Exception("Role grander user not found!");

            var toUser = await userService.GetByIdAsync(grandRoleDetails.ToUserId)
                ?? throw new Exception("User not found!");

            if (grandRoleDetails.Role > fromUser.Role)
                throw new InvalidOperationException("Access denied!");

            var userDto = new UserDTO
            {
                FirstName = toUser.FirstName,
                LastName = toUser.LastName,
                Email = toUser.Email,
                Password = toUser.Password,
                Role = grandRoleDetails.Role
            };

            await userService.UpdateAsync(toUser.Id, userDto);
            return true;
        }

        public async ValueTask<bool> UpdatePasswordAsync(Guid userId, UpdatePasswordDetails updatePasswordDetails)
        {
            var user = await userService.GetByIdAsync(userId)
                ?? throw new EntityNotFoundException(typeof(User));

            if (!passwordHasherService.Verify(updatePasswordDetails.OldPassword, user.Password))
                throw new InvalidOperationException("Password is incorrect.");

            if (updatePasswordDetails.NewPassword.Length is < 8 or > 32)
                throw new InvalidOperationException("Password leng must be between 8 and 32");

            if (updatePasswordDetails.NewPassword != updatePasswordDetails.VerifyNewPassword)
                throw new InvalidOperationException("Passwords are not same.");

            var userDto = new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = passwordHasherService.Hash(updatePasswordDetails.NewPassword),
                Role = user.Role
            };

            await userService.UpdateAsync(user.Id, userDto);

            return true;
        }
    }
}
