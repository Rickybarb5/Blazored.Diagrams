using Blazor.Flows.Components.Containers;
using Blazor.Flows.Components.Models;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Options.Behaviours;

namespace Blazor.Flows.Options.Diagram;

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