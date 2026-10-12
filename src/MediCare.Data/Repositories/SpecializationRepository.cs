using MediCare.Data.Entities;
using MediCare.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Data.Repositories;

/// <summary>
/// Specialization repository. Add methods here when a query is specific to this table.
/// </summary>
public class SpecializationRepository : Repository<Specialization>, ISpecializationRepository
{
    public SpecializationRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public Task<Specialization?> GetByNameAsync(string name)
    {
        return DbSet.FirstOrDefaultAsync(specialization => specialization.Name == name);
    }
}
