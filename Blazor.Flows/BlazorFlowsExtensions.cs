using System.Diagnostics.CodeAnalysis;
using Blazor.Flows.Services.Diagrams;
using Blazor.Flows.Services.Observers;

using Blazor.Flows.Services.Registry;
using Blazor.Flows.Services.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Flows;

/// <summary>
/// Extension class for Blazor.Flows project.
/// </summary>
public static class BlazorFlowsExtensions
{
    /// <summary>
    /// Sets up all dependencies required for Blazor.Flows.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddBlazorFlows(
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