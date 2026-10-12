using MediCare.Data.Entities;
using MediCare.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Data.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IRepository{T}"/>.
/// </summary>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected Repository(ApplicationDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    protected ApplicationDbContext Context { get; }

    protected DbSet<T> DbSet { get; }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<List<T>> ListAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }
}
