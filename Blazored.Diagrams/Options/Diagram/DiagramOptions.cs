using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Options.Behaviours;

namespace Blazored.Diagrams.Options.Diagram;

/// <inheritdoc />
public class DiagramOptions : IDiagramOptions
{
    /// <inheritdoc />
    public IDiagramStyleOptions Style { get; set; } = new DiagramStyleOptions();

    /// <inheritdoc />
    public IVirtualizationOptions Virtualization { get; init; } = new VirtualizationOptions();
    
    /// <inheritdoc />
    public CurvedLinkOptions CurvedLinkOptions { get; set; } = new();
    
    /// <inheritdoc />
    public LineLinkOptions LineLinkOptions { get; set; } = new();
    
    /// <inheritdoc />
    public OrthogonalLinkOptions OrthogonalLinkOptions { get; set; } = new();

    /// <inheritdoc />
    public List<IBehaviourOptions> BehaviourOptions { get; set; } = [];
}