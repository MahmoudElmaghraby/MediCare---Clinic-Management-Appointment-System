using MediCare.Data;
using MediCare.Services.Factories;
using MediCare.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Tests;

/// <summary>
/// Sample tests for the specialization stack. Copy this style for new services.
/// The database is EF Core InMemory, so SQL Server is not required.
/// </summary>
public class SpecializationServiceTests
{
    [Fact]
    public async Task CreateAsync_trims_name_and_lists_the_row()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var created = await service.CreateAsync("  Cardiology  ", " Heart ");

        Assert.Equal("Cardiology", created.Name);
        Assert.Equal("Heart", created.Description);
        Assert.True(created.Id > 0);

        var listed = await service.ListAsync();
        Assert.Single(listed);
        Assert.Equal("Cardiology", listed[0].Name);
    }

    [Fact]
    public async Task CreateAsync_rejects_blank_name_and_duplicate_name()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync("   ", null));

        await service.CreateAsync("Dermatology", null);

        var duplicate = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync("Dermatology", "again"));

        Assert.Contains("already exists", duplicate.Message);
        Assert.Single(await service.ListAsync());
    }

    [Fact]
    public async Task GetByNameAsync_returns_null_when_missing()
    {
        await using var context = CreateContext();
        var unitOfWork = new UnitOfWork(context);

        var found = await unitOfWork.Specializations.GetByNameAsync("Missing");

        Assert.Null(found);
    }

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static SpecializationService CreateService(ApplicationDbContext context)
    {
        return new SpecializationService(new UnitOfWork(context), new SpecializationFactory());
    }
}
