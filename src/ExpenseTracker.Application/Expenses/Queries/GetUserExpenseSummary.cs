using ExpenseTracker.Application.Common.Interfaces;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Queries;

public record GetUserExpenseSummaryQuery(
    Guid UserId,
    DateTime? StartDate = null,
    DateTime? EndDate = null) : IRequest<ExpenseSummaryDto>;

public class GetUserExpenseSummaryQueryHandler : IRequestHandler<GetUserExpenseSummaryQuery, ExpenseSummaryDto>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetUserExpenseSummaryQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }


    public async Task<ExpenseSummaryDto> Handle(GetUserExpenseSummaryQuery request, CancellationToken cancellationToken)
    {
        List<Expense> expenses;

        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            expenses = await _expenseRepository.GetExpensesByDateRangeAsync(
                request.UserId,
                request.StartDate.Value,
                request.EndDate.Value,
                cancellationToken);
        }
        else
        {
            expenses = await _expenseRepository.GetExpensesByUserIdAsync(
                request.UserId,
                cancellationToken);
        }

        if (!expenses.Any())
        {
            return new ExpenseSummaryDto(
                0,
                new Dictionary<string, decimal>(),
                0,
                null,
                null);
        }

        var totalAmount = expenses.Sum(e => e.Amount);
        var averageAmount = totalAmount / expenses.Count;
        var amountByCategory = expenses
            .GroupBy(e => e.Category)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(e => e.Amount));

        return new ExpenseSummaryDto(
            totalAmount,
            amountByCategory,
            averageAmount,
            expenses.Min(e => e.Date),
            expenses.Max(e => e.Date));
    }
}
