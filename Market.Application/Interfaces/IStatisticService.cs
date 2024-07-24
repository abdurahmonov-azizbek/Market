using Market.Domain.Models;

namespace Market.Application.Interfaces
{
    public interface IStatisticService
    {
        ValueTask<Statistics> Get(Guid userId, DateTime dateTime);
    }
}
