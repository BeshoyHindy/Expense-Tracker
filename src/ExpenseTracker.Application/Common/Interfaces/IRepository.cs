using System.Linq.Expressions;
using ExpenseTracker.Domain.Abstractions;

namespace ExpenseTracker.Application.Common.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}