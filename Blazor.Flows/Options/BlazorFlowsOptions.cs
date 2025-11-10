using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Blazor.Flows.Options;

/// <summary>
/// Options used to customize the diagram library.
/// </summary>
[ExcludeFromCodeCoverage]
public class BlazorFlowsOptions
{
    /// <summary>
    ///     This assembly  is used for automatic  component registration.
    ///     Includes the entry and executing assemblies by default.
    /// </summary>
    public List<Assembly> Assemblies { get; set; } =
        [Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly()!, typeof(BlazorFlowsExtensions).Assembly];
}