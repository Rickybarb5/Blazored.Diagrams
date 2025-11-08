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