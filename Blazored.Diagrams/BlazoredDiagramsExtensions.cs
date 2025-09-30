using System.Diagnostics.CodeAnalysis;
using Blazored.Diagrams.Services.Observers;
using Blazored.Diagrams.Services.Providers;
using Blazored.Diagrams.Services.Registry;
using Blazored.Diagrams.Services.Serialization;
using Blazored.Diagrams.Services.Virtualization;
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
            .AddSingleton<IDiagramServiceProvider, DiagramServiceProvider>()
            .AddSingleton<IVirtualizationService, VirtualizationService>()
            .AddSingleton<ISerializationService, SerializationService>()
            .AddSingleton<IComponentRegistry, ComponentRegistry>();
        
        return services;
    }
}