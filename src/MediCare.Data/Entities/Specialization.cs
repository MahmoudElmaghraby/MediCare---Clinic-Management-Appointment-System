namespace MediCare.Data.Entities;

/// <summary>
/// A clinic specialty such as Cardiology or Dermatology.
/// Copy this shape when you add Doctor, Patient, or Appointment.
/// </summary>
public class Specialization : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
