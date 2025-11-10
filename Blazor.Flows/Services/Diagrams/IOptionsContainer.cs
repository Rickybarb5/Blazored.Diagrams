using Blazor.Flows.Components.Models;
using Blazor.Flows.Options.Behaviours;
using Blazor.Flows.Options.Diagram;

namespace Blazor.Flows.Services.Diagrams;

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