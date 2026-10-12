using MediCare.Data.Entities;

namespace MediCare.Services.Interfaces;

/// <summary>
/// Application operations for specializations. Controllers should depend on this interface.
/// </summary>
public interface ISpecializationService
{
    Task<List<Specialization>> ListAsync();

    Task<Specialization> CreateAsync(string name, string? description);
}
