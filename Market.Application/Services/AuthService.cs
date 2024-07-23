using Market.Application.Interfaces;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Models;

namespace Market.Application.Services;

public class AuthService(
    IUserService userService,
    IPasswordHasherService passwordHasherService,
    ITokenService tokenService) : IAuthService
{
    public async ValueTask<string> LoginAsync(LoginDetails loginDetails)
    {
        var user = (await userService.GetAllAsync()).FirstOrDefault(user =>
            user.Email == loginDetails.Email)
                ?? throw new EntityNotFoundException(typeof(User));

        if (!passwordHasherService.Verify(loginDetails.Password, user.Password))
            throw new InvalidOperationException("Invalid email or password.");

        var token = tokenService.GetToken(user);

        return token;
    }

    public async ValueTask<bool> RegisterAsync(RegisterDetails registerDetails)
    {
        if (registerDetails.Password.Length is < 8 or > 32)
            throw new ArgumentException("Password must be between 8 and 32");

        var user = (await userService.GetAllAsync()).FirstOrDefault(user =>
            user.Email == registerDetails.Email);

        if (user is not null)
            throw new InvalidOperationException("User already exists with this email!");

        var creationUserDto = new UserDTO
        {
            FirstName = registerDetails.FirstName,
            LastName = registerDetails.LastName,
            Email = registerDetails.Email,
            Password = passwordHasherService.Hash(registerDetails.Password),
            Role = Domain.Enums.Role.Admin,
        };

        await userService.CreateAsync(creationUserDto);

        return true;
    }
}
