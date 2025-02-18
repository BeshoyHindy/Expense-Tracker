using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Commands;

public record UpdateExpenseCommand(
    Guid Id,
    decimal Amount,
    string Category) : IRequest;

public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Expense ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required")
            .MaximumLength(50)
            .WithMessage("Category must not exceed 50 characters");
    }
}

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (expense is null)
        {
            throw new NotFoundException($"Expense with ID {request.Id} not found");
        }

        expense.Update(request.Amount, request.Category);
        
        _expenseRepository.Update(expense);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}