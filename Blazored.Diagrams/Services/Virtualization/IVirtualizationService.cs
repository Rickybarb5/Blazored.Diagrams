using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Services.Virtualization;

/// <summary>
///     Feature set for virtualization.
/// </summary>
public interface IVirtualizationService
{
    /// <summary>
    /// Takes into account virtualization.
    /// If enabled it only items that are visible on the current viewport.
    /// If disabled it renders everything.
    /// </summary>
    /// <typeparam name="T">Model must have size and position.</typeparam>
    /// <param name="diagram">Diagram instance</param>
    /// <param name="items">items to check for virtualization.</param>
    /// <returns>A list of items that will appear when virtualization is on.</returns>
    public IEnumerable<T> Virtualize<T>(IDiagram diagram, IEnumerable<T> items) where T : IPosition, ISize;
}