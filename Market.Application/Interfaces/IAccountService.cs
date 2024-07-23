using Market.Domain.Models;

namespace Market.Application.Interfaces;

public interface IAccountService
{
    ValueTask<bool> UpdatePasswordAsync(long userId, UpdatePasswordDetails updatePasswordDetails);
    ValueTask<bool> GrandRoleAsync(GrandRoleDetails grandRoleDetails);
}
