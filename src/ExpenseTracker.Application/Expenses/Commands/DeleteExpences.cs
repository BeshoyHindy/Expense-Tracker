using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Commands;

public record DeleteExpenseCommand(Guid Id) : IRequest;

public class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Expense ID is required");
    }
}

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (expense is null)
        {
            throw new NotFoundException($"Expense with ID {request.Id} not found");
        }

        _expenseRepository.Remove(expense);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
