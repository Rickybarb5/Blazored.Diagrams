using Blazored.Diagrams.Components.Models;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Options.Diagram;

namespace Blazored.Diagrams.Services.Diagrams;

/// <summary>
/// Access diagram options.
/// </summary>
public interface IOptionsContainer
{
    /// <summary>
    /// Styling options for the diagram.
    /// </summary>
    IDiagramStyleOptions Styling { get; }

    /// <summary>
    /// Virtualization options for the diagram.
    /// </summary>
    IVirtualizationOptions Virtualization { get; }
    
    /// <summary>
    /// Options for <see cref="CurvedLinkComponent"/>.
    /// </summary>
    public CurvedLinkOptions CurvedLinkOptions { get; }

    /// <summary>
    /// Options for <see cref="LineLinkComponent"/>.
    /// </summary>
    public LineLinkOptions LineLinkOptions { get; }

    /// <summary>
    /// Options for <see cref="OrthogonalLinkComponent"/>.
    /// </summary>
    public OrthogonalLinkOptions OrthogonalLinkOptions { get; }
}