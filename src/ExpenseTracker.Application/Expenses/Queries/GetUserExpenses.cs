using ExpenseTracker.Application.Common.Interfaces;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Queries;

public record GetUserExpensesQuery(
    Guid UserId,
    DateTime? StartDate = null,
    DateTime? EndDate = null) : IRequest<List<ExpenseDto>>;

public class GetUserExpensesQueryHandler : IRequestHandler<GetUserExpensesQuery, List<ExpenseDto>>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetUserExpensesQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<List<ExpenseDto>> Handle(GetUserExpensesQuery request, CancellationToken cancellationToken)
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

        return expenses.Select(expense => new ExpenseDto(
            expense.Id,
            expense.Amount,
            expense.Category,
            expense.Description,
            expense.Date,
            expense.UserId)).ToList();
    }
}