namespace MediCare.Data.Interfaces;

/// <summary>
/// Groups repositories and saves them in one database call.
/// </summary>
public interface IUnitOfWork
{
    ISpecializationRepository Specializations { get; }

    Task<int> SaveChangesAsync();
}
