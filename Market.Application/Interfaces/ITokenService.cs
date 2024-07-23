using Market.Domain.Entities;

namespace Market.Application.Interfaces
{
    public interface ITokenService
    {
        string GetToken(User user);
    }
}
