using MediCare.Data.Interfaces;
using MediCare.Data.Repositories;

namespace MediCare.Data;

/// <summary>
/// One unit of work per web request. Dependency injection owns the context lifetime,
/// so this class does not dispose the context.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Specializations = new SpecializationRepository(context);
    }

    public ISpecializationRepository Specializations { get; }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
