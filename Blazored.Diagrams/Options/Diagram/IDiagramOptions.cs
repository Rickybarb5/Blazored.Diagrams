using Blazored.Diagrams.Components.Containers;
using Blazored.Diagrams.Components.Models;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Options.Behaviours;

namespace Blazored.Diagrams.Options.Diagram;

/// <summary>
/// Default Diagram options.
/// </summary>
public interface IDiagramOptions
{
    /// <summary>
    /// Style options for <see cref="DiagramContainer"/>
    /// </summary>
    IDiagramStyleOptions Style { get; set; }
    
    /// <summary>
    /// Virtualization options for <see cref="DiagramContainer"/>.
    /// Usefull if there are many components on the diagram.
    /// Disabled by default.
    /// </summary>
    IVirtualizationOptions Virtualization { get; init; }

    /// <summary>
    /// Option list for the behaviours.
    /// </summary>
    List<IBehaviourOptions> BehaviourOptions { get; set; }

    /// <summary>
    /// Options for <see cref="CurvedLinkComponent"/>.
    /// </summary>
    CurvedLinkOptions CurvedLinkOptions { get; set; }

    /// <summary>
    /// Options for <see cref="LineLinkComponent"/>.
    /// </summary>
    LineLinkOptions LineLinkOptions { get; set; }

    /// <summary>
    /// Options for <see cref="OrthogonalLinkComponent"/>.
    /// </summary>
    OrthogonalLinkOptions OrthogonalLinkOptions { get; set; }
}