using Blazor.Flows.Behaviours;
using Blazor.Flows.Diagrams;
using Blazor.Flows.Links;

namespace Blazor.Flows.Events;

/// <summary>
/// Event triggered when the panning starts in the <see cref="PanBehaviour"/>
/// </summary>
/// <param name="Diagram">Diagram that will be panned.</param>
/// <param name="PanX">Current X value of the pan</param>
/// <param name="PanY">Current Y Value of the pan.</param>
public record PanStartEvent(IDiagram Diagram, int PanX, int PanY) : IEvent;

/// <summary>
/// Event triggered when the panning ends in the <see cref="PanBehaviour"/>
/// </summary>
/// <param name="Diagram">Diagram affected by the pan.</param>
/// <param name="PanX">Current X value of the pan</param>
/// <param name="PanY">Current Y Value of the pan.</param>
public record PanEndEvent(IDiagram Diagram, int PanX, int PanY) : IEvent;

/// <summary>
/// Event triggered when a new link is being created via the <see cref="DrawLinkBehavior"/>
/// At this stage the link has a source port, but no target port, only target position.
/// </summary>
/// <param name="Link">Link being created.</param>
public record DrawLinkStartEvent(ILink Link) : IEvent;

/// <summary>
/// Event triggered when a new link has finished being created via the <see cref="DrawLinkBehavior"/>
/// At this stage the link has a source and target  port.
/// </summary>
/// <param name="Link">Created link</param>
public record DrawLinkCreatedEvent(ILink Link) : IEvent;

/// <summary>
/// Event triggered when a link was being created  via the <see cref="DrawLinkBehavior"/> but was not attached to a target port.
/// </summary>
/// <param name="Link">Link whose creation was cancelled .</param>
public record DrawLinkCancelledEvent(ILink Link) : IEvent;

/// <summary>
/// Enabled event for behaviours.
/// </summary>
/// <param name="BehaviourType">Type of the behaviour that was enabled or disabled.</param>
/// <param name="IsEnabled">Value indicating whether the behaviour was enabled or disabled.</param>
public record BehaviourEnabledEvent(Type BehaviourType, bool IsEnabled):IEvent;