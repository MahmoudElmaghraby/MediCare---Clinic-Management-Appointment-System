using MediCare.Data.Entities;
using MediCare.Data.Interfaces;
using MediCare.Services.Factories;
using MediCare.Services.Interfaces;

namespace MediCare.Services.Implementations;

/// <summary>
/// Specialization use cases. Talks to the database only through <see cref="IUnitOfWork"/>.
/// </summary>
public class SpecializationService : ISpecializationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SpecializationFactory _factory;

    public SpecializationService(IUnitOfWork unitOfWork, SpecializationFactory factory)
    {
        _unitOfWork = unitOfWork;
        _factory = factory;
    }

    public Task<List<Specialization>> ListAsync()
    {
        return _unitOfWork.Specializations.ListAsync();
    }

    public async Task<Specialization> CreateAsync(string name, string? description)
    {
        var specialization = _factory.Create(name, description);

        var existing = await _unitOfWork.Specializations.GetByNameAsync(specialization.Name);
        if (existing is not null)
        {
            throw new InvalidOperationException(
                "A specialization with this name already exists.");
        }

        await _unitOfWork.Specializations.AddAsync(specialization);
        await _unitOfWork.SaveChangesAsync();

        return specialization;
    }
}
