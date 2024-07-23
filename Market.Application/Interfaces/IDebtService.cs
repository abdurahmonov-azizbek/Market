using Market.Domain.DTOs;
using Market.Domain.Entities;

namespace Market.Application.Interfaces;

public interface IDebtService
{
    ValueTask<Debt> CreateAsync(long userId, DebtDTO debtDTO);
    ValueTask<IQueryable<Debt>> GetAll(long userId);
    ValueTask<Debt> GetByIdAsync(long debtId);
    ValueTask<Debt> UpdateAsync(long debtId, DebtDTO debtDTO);
    ValueTask<Debt> DeleteByIdAsync(long debtId);
}
