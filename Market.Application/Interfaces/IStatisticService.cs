using Market.Domain.Models;

namespace Market.Application.Interfaces
{
    public interface IStatisticService
    {
        ValueTask<Statistics> GetFull(Guid userId, DateTime dateTime);
        ValueTask<Statistics> GetFull(Guid userId);
        ValueTask<OrderStatistics> Get(Guid userId, DateTime dateTime);
        ValueTask<OrderStatistics> Get(Guid userId);
    }
}
