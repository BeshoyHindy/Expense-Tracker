using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Common.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Queries;

public record GetExpenseByIdQuery(Guid Id) : IRequest<ExpenseDto>;

public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, ExpenseDto>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetExpenseByIdQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<ExpenseDto> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken);

        if (expense is null)
        {
            throw new NotFoundException($"Expense with ID {request.Id} not found");
        }

        return new ExpenseDto(
            expense.Id,
            expense.Amount,
            expense.Category,
            expense.Description,
            expense.Date,
            expense.UserId);
    }
}
