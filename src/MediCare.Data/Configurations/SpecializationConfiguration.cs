using MediCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Data.Configurations;

/// <summary>
/// Maps <see cref="Specialization"/> to the Specializations table.
/// </summary>
public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
{
    public void Configure(EntityTypeBuilder<Specialization> builder)
    {
        builder.ToTable("Specializations");

        builder.Property(specialization => specialization.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(specialization => specialization.Description)
            .HasMaxLength(500);

        builder.HasIndex(specialization => specialization.Name)
            .IsUnique();
    }
}
