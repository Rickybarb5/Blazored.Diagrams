using System.Diagnostics.CodeAnalysis;
using Blazored.Diagrams.Services.Diagrams;
using Blazored.Diagrams.Services.Observers;

using Blazored.Diagrams.Services.Registry;
using Blazored.Diagrams.Services.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Blazored.Diagrams;

/// <summary>
/// Extension class for Blazored diagrams.
/// </summary>
public static class BlazoredDiagramsExtensions
{
    /// <summary>
    /// Sets up all dependencies required for Blazored.Diagrams.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddBlazoredDiagrams(
        this IServiceCollection services)
    {
        services
            .AddSingleton<IResizeObserverService, ResizeObserverService>()
            .AddSingleton<ISerializationService, SerializationService>()
            .AddTransient<IDiagramService, DiagramService>()
            .AddSingleton<IComponentRegistry, ComponentRegistry>();
        
        return services;
    }
}