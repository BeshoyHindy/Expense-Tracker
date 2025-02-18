using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace ExpenseTracker.Domain.Abstractions;


public abstract class Entity() : Entity<Guid>(Guid.NewGuid());

public abstract class Entity<TId>
{
    public TId Id { get; protected init; }
    
    private List<IDomainEvent>? _domainEvents;
    protected Entity(TId id) => Id = id;
    protected Entity() { }
    
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => (_domainEvents ??= []).AsReadOnly();
    public IReadOnlyCollection<IDomainEvent> PopDomainEvents()
    {
        var events = _domainEvents?.ToList() ?? [];
        _domainEvents?.Clear();
        return events;
    }
    public void AddDomainEvent(IDomainEvent domainEvent) =>  (_domainEvents ??= []).Add(domainEvent);
    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents?.Remove(domainEvent);
    protected void ClearDomainEvents() => _domainEvents?.Clear();
}

public interface IDomainEvent : INotification
{
}