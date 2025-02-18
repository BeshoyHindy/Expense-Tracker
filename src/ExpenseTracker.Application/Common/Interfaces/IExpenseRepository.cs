using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Common.Interfaces;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<List<Expense>> GetExpensesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<List<Expense>> GetExpensesByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken = default);

    Task<decimal> GetTotalExpensesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}