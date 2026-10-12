namespace MediCare.Data.Entities;

/// <summary>
/// Shared key for every table entity. New entities should inherit this class.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
}
