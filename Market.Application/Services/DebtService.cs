using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;

namespace Market.Application.Services;

public class DebtService(AppDbContext dbContext) : IDebtService
{
    public async ValueTask<Debt> CreateAsync(long userId, DebtDTO debtDTO)
    {
        var debt = new Debt
        {
            Id = dbContext.Debts.Count() + 1,
            Title = debtDTO.Title,
            Type = debtDTO.Type,
            Price = debtDTO.Price,
            UserId = userId,
            CreatedDate = DateTime.Now
        };

        await dbContext.Debts.AddAsync(debt);
        await dbContext.SaveChangesAsync();

        return debt;
    }

    public async ValueTask<Debt> DeleteByIdAsync(long debtId)
    {
        var debt = await dbContext.Debts.FindAsync(debtId)
            ?? throw new EntityNotFoundException(typeof(Debt));

        debt.IsDeleted = true;
        debt.DeletedDate = DateTime.Now;

        dbContext.Debts.Update(debt);
        await dbContext.SaveChangesAsync();

        return debt;
    }

    public ValueTask<IQueryable<Debt>> GetAll(long userId)
        => new(dbContext.Debts.Where(debt => debt.UserId == userId));

    public async ValueTask<Debt> GetByIdAsync(long debtId)
    {
        var debt = await dbContext.Debts.FindAsync(debtId)
            ?? throw new EntityNotFoundException(typeof(Debt));

        return debt;
    }

    public async ValueTask<Debt> UpdateAsync(long debtId, DebtDTO debtDTO)
    {
        var debt = await dbContext.Debts.FindAsync(debtId)
            ?? throw new EntityNotFoundException(typeof(Debt));

        debt.Title = debtDTO.Title;
        debt.Price = debtDTO.Price;
        debt.Type = debtDTO.Type;

        dbContext.Debts.Update(debt);
        await dbContext.SaveChangesAsync();

        return debt;
    }
}
