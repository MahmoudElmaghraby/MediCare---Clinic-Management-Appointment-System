using MediCare.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Data;

/// <summary>
/// EF Core entry point. Identity tables are not mapped yet.
/// When that task starts, this class can inherit IdentityDbContext.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Specialization> Specializations => Set<Specialization>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
