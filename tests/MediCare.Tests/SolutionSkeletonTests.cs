using System.Reflection;

namespace MediCare.Tests;

public class SolutionSkeletonTests
{
    [Fact]
    public void Web_project_exposes_home_controller()
    {
        var controller = typeof(MediCare.Web.Controllers.HomeController);
        Assert.Equal("MediCare.Web", controller.Assembly.GetName().Name);
    }

    [Theory]
    [InlineData("MediCare.Services")]
    [InlineData("MediCare.Data")]
    public void Class_library_assemblies_load(string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        Assert.Equal(assemblyName, assembly.GetName().Name);
    }
}
