using Market.Domain.Models;

namespace Market.Application.Interfaces;

public interface IProductOrchestrationService
{
    ValueTask<FullProduct> GetByCodeAsync(long code);
    ValueTask<List<FullProduct>> GetByKeyAsync(string key);
}
