using Market.Application.Interfaces;
using Market.Data.DbContexts;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Extensions;

namespace Market.Application.Services;

public class DebtService(AppDbContext dbContext) : IDebtService
{
    public async ValueTask<Debt> CreateAsync(Guid userId, DebtDTO debtDTO)
    {
        var debt = new Debt
        {
            Id = Guid.NewGuid(),
            Title = debtDTO.Title,
            Type = debtDTO.Type,
            Price = debtDTO.Price,
            UserId = userId,
            CreatedDate = Helper.GetCurrentDateTime()
        };

        await dbContext.Debts.AddAsync(debt);
        await dbContext.SaveChangesAsync();

        return debt;
    }

    public async ValueTask<Debt> DeleteByIdAsync(Guid debtId)
    {
        var debt = await dbContext.Debts.FindAsync(debtId)
            ?? throw new EntityNotFoundException(typeof(Debt));

        debt.IsDeleted = true;
        debt.DeletedDate = Helper.GetCurrentDateTime();

        dbContext.Debts.Update(debt);
        await dbContext.SaveChangesAsync();

        return debt;
    }

    public ValueTask<IQueryable<Debt>> GetAll(Guid userId)
        => new(dbContext.Debts.Where(debt => debt.UserId == userId));

    public async ValueTask<Debt> GetByIdAsync(Guid debtId)
    {
        var debt = await dbContext.Debts.FindAsync(debtId)
            ?? throw new EntityNotFoundException(typeof(Debt));

        return debt;
    }

    public async ValueTask<Debt> UpdateAsync(Guid debtId, DebtDTO debtDTO)
    {
        var debt = await dbContext.Debts.FindAsync(debtId)
            ?? throw new EntityNotFoundException(typeof(Debt));

        debt.Title = debtDTO.Title;
        debt.Price = debtDTO.Price;
        debt.Type = debtDTO.Type;
        debt.UpdatedDate = Helper.GetCurrentDateTime();

        dbContext.Debts.Update(debt);
        await dbContext.SaveChangesAsync();

        return debt;
    }
}
