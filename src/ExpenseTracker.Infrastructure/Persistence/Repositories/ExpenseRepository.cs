using ExpenseTracker.Application.Common.Interfaces;
using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories;

public class ExpenseRepository : Repository<Expense>, IExpenseRepository
{
    public ExpenseRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<List<Expense>> GetExpensesByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(e => e.UserId == userId)
            .OrderByDescending(e => e.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Expense>> GetExpensesByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(e => e.UserId == userId
                        && e.Date >= startDate
                        && e.Date <= endDate)
            .OrderByDescending(e => e.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalExpensesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(e => e.UserId == userId)
            .SumAsync(e => e.Amount, cancellationToken);
    }
}