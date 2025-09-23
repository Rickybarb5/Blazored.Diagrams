using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Blazored.Diagrams.Options;

/// <summary>
/// Options used to customize the diagram library.
/// </summary>
[ExcludeFromCodeCoverage]
public class BlazoredDiagramsOptions
{
    /// <summary>
    ///     This assembly  is used for automatic  component registration.
    ///     Includes the entry and executing assemblies by default.
    /// </summary>
    public List<Assembly> Assemblies { get; set; } =
        [Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly()!, typeof(BlazoredDiagramsExtensions).Assembly];
}