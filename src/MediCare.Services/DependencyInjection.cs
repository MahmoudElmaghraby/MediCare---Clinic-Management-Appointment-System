using MediCare.Data;
using MediCare.Services.Factories;
using MediCare.Services.Implementations;
using MediCare.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediCare.Services;

/// <summary>
/// Composition helper used by Program.cs.
/// The web project calls this method and does not reference MediCare.Data.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddMediCareServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediCareData(configuration);

        services.AddSingleton<SpecializationFactory>();
        services.AddScoped<ISpecializationService, SpecializationService>();

        return services;
    }
}
