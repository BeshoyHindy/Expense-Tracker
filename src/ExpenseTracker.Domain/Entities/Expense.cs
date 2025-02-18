using ExpenseTracker.Domain.Abstractions;
using ExpenseTracker.Domain.Events;

namespace ExpenseTracker.Domain.Entities;

public class Expense : Entity
{
    public decimal Amount { get; private set; }
    public string Category { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public Guid UserId { get; private set; }
    
    private Expense(decimal amount, string category, string description, DateTime date, Guid userId)
    {
        Amount = amount;
        Category = category;
        Description = description;
        Date = date;
        UserId = userId;
    }

    public static Expense Create(
        decimal amount,
        string category,
        string description,
        DateTime date,
        Guid userId)
    {
        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero");

        if (string.IsNullOrWhiteSpace(category))
            throw new DomainException("Category is required");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Description is required");

        var expense = new Expense(
            amount,
            category,
            description,
            date,
            userId);

        expense.AddDomainEvent(new ExpenseCreatedDomainEvent(expense.Id));

        return expense;
    }
    
    public void Update(decimal amount, string category)
    {
        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero");

        if (string.IsNullOrWhiteSpace(category))
            throw new DomainException("Category is required");

        Amount = amount;
        Category = category;

        AddDomainEvent(new ExpenseUpdatedDomainEvent(Id));
    }
}