using MediCare.Data.Entities;

namespace MediCare.Services.Factories;

/// <summary>
/// Builds a valid specialization. Services call this instead of setting properties ad hoc.
/// </summary>
public class SpecializationFactory
{
    public Specialization Create(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        var trimmedDescription = string.IsNullOrWhiteSpace(description)
            ? null
            : description.Trim();

        return new Specialization
        {
            Name = name.Trim(),
            Description = trimmedDescription
        };
    }
}
