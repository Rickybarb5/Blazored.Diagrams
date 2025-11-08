using System.Diagnostics.CodeAnalysis;

namespace Blazored.Diagrams.Services.Observers;

/// <summary>
/// Helper class for the observer logic.
/// </summary>
[ExcludeFromCodeCoverage]
public class ResizeObserverEntry
{
    /// <summary>
    /// New width of the element
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// New height of the element
    /// </summary>
    public double Height { get; set; }
}