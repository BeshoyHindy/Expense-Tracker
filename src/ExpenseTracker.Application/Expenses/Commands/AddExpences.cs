using ExpenseTracker.Application.Common.Interfaces;
using ExpenseTracker.Domain.Entities;
using FluentValidation;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Commands;

public record AddExpenseCommand(
    decimal Amount,
    string Category,
    string Description,
    DateTime Date,
    Guid UserId) : IRequest<Guid>;


public class AddExpenseCommandHandler : IRequestHandler<AddExpenseCommand, Guid>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = Expense.Create(
            request.Amount,
            request.Category,
            request.Description,
            request.Date,
            request.UserId);

        _expenseRepository.Add(expense);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return expense.Id;
    }
}

public class AddExpenseCommandValidator : AbstractValidator<AddExpenseCommand>
{
    public AddExpenseCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required")
            .MaximumLength(50)
            .WithMessage("Category must not exceed 50 characters");

 

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Date cannot be in the future");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}