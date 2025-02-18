using ExpenseTracker.Domain.Abstractions;

namespace ExpenseTracker.Domain.Events;

public record ExpenseCreatedDomainEvent(Guid ExpenseId) : IDomainEvent;

public record ExpenseUpdatedDomainEvent(Guid ExpenseId) : IDomainEvent;