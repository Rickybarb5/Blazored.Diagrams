using System.Diagnostics.CodeAnalysis;
using Blazored.Diagrams.Services;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Observers;
using Blazored.Diagrams.Services.Providers;
using Blazored.Diagrams.Services.Registry;
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
            .AddTransient<IVirtualizationService, VirtualizationService>()
            .AddTransient<IComponentRegistry, ComponentRegistry>();
        
        return services;
    }
}