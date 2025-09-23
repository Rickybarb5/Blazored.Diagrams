using System.Diagnostics.CodeAnalysis;
using Blazored.Diagrams.Services;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Observers;
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
    /// <param name="configureOptions">Options builder for the diagram options.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddBlazoredDiagrams(
        this IServiceCollection services)
    {
        services
            .AddTransient<IDiagramService, DiagramService>()
            .AddSingleton<IResizeObserverService, ResizeObserverService>()
            .AddTransient<IVirtualizationService, VirtualizationService>()
            .AddTransient<IComponentRegistry, ComponentRegistry>()
            .AddTransient<IEventAggregator, EventAggregator>();

        return services;
    }
}