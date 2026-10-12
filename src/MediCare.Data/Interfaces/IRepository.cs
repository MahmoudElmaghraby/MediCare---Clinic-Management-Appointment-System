using MediCare.Data.Entities;

namespace MediCare.Data.Interfaces;

/// <summary>
/// Generic data access. Does not call SaveChanges. The unit of work saves.
/// </summary>
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);

    Task<List<T>> ListAsync();

    Task AddAsync(T entity);

    void Update(T entity);

    void Remove(T entity);
}
