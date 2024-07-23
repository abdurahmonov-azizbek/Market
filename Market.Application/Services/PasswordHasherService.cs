using Market.Application.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace Market.Application.Services;

public class PasswordHasherService : IPasswordHasherService
{
    public string Hash(string password)
        => BC.HashPassword(password);

    public bool Verify(string password, string hash)
        => BC.Verify(password, hash);
}
