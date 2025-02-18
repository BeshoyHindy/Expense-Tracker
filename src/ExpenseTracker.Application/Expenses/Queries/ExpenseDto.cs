namespace ExpenseTracker.Application.Expenses.Queries;

public record ExpenseDto(
    Guid Id,
    decimal Amount,
    string Category,
    string Description,
    DateTime Date,
    Guid UserId);
    
public record ExpenseSummaryDto(
    decimal TotalAmount,
    Dictionary<string, decimal> AmountByCategory,
    decimal AverageAmount,
    DateTime? FirstExpenseDate,
    DateTime? LastExpenseDate);
    
public record CategoryExpenseDto(
    string Category,
    decimal TotalAmount,
    int Count,
    decimal AverageAmount);
