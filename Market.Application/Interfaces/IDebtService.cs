using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface IDebtService
{
    ValueTask<Debt> CreateAsync(Guid userId, DebtDTO debtDTO);
    ValueTask<IQueryable<Debt>> GetAll(Guid userId);
    ValueTask<Debt> GetByIdAsync(Guid debtId);
    ValueTask<Debt> UpdateAsync(Guid debtId, DebtDTO debtDTO);
    ValueTask<Debt> DeleteByIdAsync(Guid debtId);
}
