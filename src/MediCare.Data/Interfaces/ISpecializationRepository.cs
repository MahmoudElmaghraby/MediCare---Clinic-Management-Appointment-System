using MediCare.Data.Entities;

namespace MediCare.Data.Interfaces;

/// <summary>
/// Specialization queries that do not belong on the generic repository.
/// </summary>
public interface ISpecializationRepository : IRepository<Specialization>
{
    Task<Specialization?> GetByNameAsync(string name);
}
