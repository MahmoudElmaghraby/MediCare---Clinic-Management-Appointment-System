using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MediCare.Data;

/// <summary>
/// Creates a context for dotnet ef without starting the web app.
/// The string is only a design time placeholder. Runtime uses appsettings.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public const string DesignTimeConnectionString =
        "Server=localhost;Database=MediCare;Trusted_Connection=True;TrustServerCertificate=True";

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(DesignTimeConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
