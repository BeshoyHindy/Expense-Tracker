using ExpenseTracker.Application.Common.Interfaces;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Queries;

public record GetExpensesByCategoryQuery(
    Guid UserId,
    DateTime? StartDate = null,
    DateTime? EndDate = null) : IRequest<List<CategoryExpenseDto>>;

public class GetExpensesByCategoryQueryHandler : IRequestHandler<GetExpensesByCategoryQuery, List<CategoryExpenseDto>>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetExpensesByCategoryQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<List<CategoryExpenseDto>> Handle(GetExpensesByCategoryQuery request, CancellationToken cancellationToken)
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

        return expenses
            .GroupBy(e => e.Category)
            .Select(g => new CategoryExpenseDto(
                g.Key,
                g.Sum(e => e.Amount),
                g.Count(),
                g.Average(e => e.Amount)))
            .OrderByDescending(c => c.TotalAmount)
            .ToList();
    }
}