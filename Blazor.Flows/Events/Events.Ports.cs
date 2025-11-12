using Blazor.Flows.Interfaces;
using Blazor.Flows.Links;
using Blazor.Flows.Ports;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Flows.Events;

// Port Events

/// <summary>
/// Base class for a port event.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that triggered the event.</param>
public record PortEvent(IPort Model) : ModelEventBase<IPort>(Model);

/// <summary>
/// Event triggered when a port is added to a node or group.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that was added.</param>
public record PortAddedEvent(IPort Model) : PortEvent(Model);

/// <summary>
/// Event triggered when a port is removed from a node or group.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that was removed.</param>
public record PortRemovedEvent(IPort Model) : PortEvent(Model);

/// <summary>
/// Event triggered when a port's position changes relative to its parent.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that moved.</param>
/// <param name="OldX">The previous X position value.</param>
/// <param name="OldY">The previous Y position value.</param>
/// <param name="NewX">The new X position value.</param>
/// <param name="NewY">The new Y position value.</param>
public record PortPositionChangedEvent(IPort Model, double OldX, double OldY, double NewX, double NewY)
    : PortEvent(Model);

/// <summary>
/// Event triggered when the visual alignment of the port changes (e.g., from Left to Top).
/// </summary>
/// <param name="Model">The <see cref="IPort"/> whose alignment changed.</param>
/// <param name="OldPosition">The previous <see cref="Ports.PortAlignment"/>.</param>
/// <param name="NewPosition">The new <see cref="Ports.PortAlignment"/>.</param>
public record PortAlignmentChangedEvent(IPort Model, PortAlignment OldPosition, PortAlignment NewPosition)
    : PortEvent(Model);

/// <summary>
/// Event triggered when the visibility state of a port changes.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> whose visibility state changed.</param>
public record PortVisibilityChangedEvent(IPort Model) : PortEvent(Model);

/// <summary>
/// Event triggered when a port is moved from one container (node or group) to another.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> whose parent changed.</param>
/// <param name="OldParent">The previous container that held the port.</param>
/// <param name="NewParent">The new container that now holds the port.</param>
public record PortParentChangedEvent(IPort Model, IPortContainer OldParent, IPortContainer NewParent)
    : PortEvent(Model);

/// <summary>
/// Event triggered when an incoming link is added to the port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> the link was added to.</param>
/// <param name="AddedLink">The <see cref="ILink"/> that was added.</param>
public record IncomingLinkAddedEvent(IPort Model, ILink AddedLink) : PortEvent(Model);

/// <summary>
/// Event triggered when an incoming link is removed from the port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> the link was removed from.</param>
/// <param name="RemovedLink">The <see cref="ILink"/> that was removed.</param>
public record IncomingLinkRemovedEvent(IPort Model, ILink RemovedLink) : PortEvent(Model);

/// <summary>
/// Event triggered when an outgoing link is added to the port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> the link was added to.</param>
/// <param name="AddedLink">The <see cref="ILink"/> that was added.</param>
public record OutgoingLinkAddedEvent(IPort Model, ILink AddedLink) : PortEvent(Model);

/// <summary>
/// Event triggered when an outgoing link is removed from the port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> the link was removed from.</param>
/// <param name="RemovedLink">The <see cref="ILink"/> that was removed.</param>
public record OutgoingLinkRemovedEvent(IPort Model, ILink RemovedLink) : PortEvent(Model);

/// <summary>
/// Event triggered when the size (width or height) of a port changes.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> whose size changed.</param>
/// <param name="OldWidth">The previous width value.</param>
/// <param name="OldHeight">The previous height value.</param>
/// <param name="NewWidth">The new width value.</param>
/// <param name="NewHeight">The new height value.</param>
public record PortSizeChangedEvent(IPort Model, double OldWidth, double OldHeight, double NewWidth, double NewHeight)
    : PortEvent(Model);

/// <summary>
/// Event triggered when the justification (e.g., start, center, end) of a port changes.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> whose justification changed.</param>
/// <param name="OldJustification">The previous <see cref="Ports.PortJustification"/>.</param>
/// <param name="NewJustification">The new <see cref="Ports.PortJustification"/>.</param>
public record PortJustificationChangedEvent(
    IPort Model,
    PortJustification OldJustification,
    PortJustification NewJustification)
    : PortEvent(Model);

/// <summary>
/// Event triggered when a pointer (e.g., mouse or touch) is pressed down on a port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record PortPointerDownEvent(IPort Model, PointerEventArgs Args) : ModelInputEvent<IPort>(Model);

/// <summary>
/// Event triggered when a pointer is released from a port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record PortPointerUpEvent(IPort Model, PointerEventArgs Args) : ModelInputEvent<IPort>(Model);

/// <summary>
/// Event triggered when a pointer moves over a port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record PortPointerMoveEvent(IPort Model, PointerEventArgs Args) : ModelInputEvent<IPort>(Model);

/// <summary>
/// Event triggered when a pointer enters the bounds of a port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record PortPointerEnterEvent(IPort Model, PointerEventArgs Args) : ModelInputEvent<IPort>(Model);

/// <summary>
/// Event triggered when a pointer leaves the bounds of a port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record PortPointerLeaveEvent(IPort Model, PointerEventArgs Args) : ModelInputEvent<IPort>(Model);

/// <summary>
/// Event triggered when the mouse wheel is scrolled while over a port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that triggered the event.</param>
/// <param name="Args">The <see cref="WheelEventArgs"/>.</param>
public record PortWheelEvent(IPort Model, WheelEventArgs Args) : ModelInputEvent<IPort>(Model);

/// <summary>
/// Event triggered when an <c>@onclick</c> event occurs on a port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that triggered the event.</param>
/// <param name="Args">The <see cref="MouseEventArgs"/>.</param>
public record PortClickedEvent(IPort Model, MouseEventArgs Args) : ModelInputEvent<IPort>(Model);

/// <summary>
/// Event triggered when an <c>@ondblclick</c> event occurs on a port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that triggered the event.</param>
/// <param name="Args">The <see cref="MouseEventArgs"/>.</param>
public record PortDoubleClickedEvent(IPort Model, MouseEventArgs Args) : ModelInputEvent<IPort>(Model);

/// <summary>
/// Event triggered when a redraw is manually requested for a port.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that requested the redraw.</param>
public record PortRedrawEvent(IPort Model) : PortEvent(Model);

/// <summary>
/// Event triggered when a port is selected.
/// </summary>
/// <param name="Model">The <see cref="IPort"/> whose selection status changed.</param>
public record PortSelectionChangedEvent(IPort Model) : PortEvent(Model);

/// <summary>
/// Event triggered when <see cref="IPort.ZIndex"/> changes;
/// </summary>
/// <param name="Model">The <see cref="IPort"/> that triggered the event.</param>
public record PortZIndexChanged(IPort Model): PortEvent(Model);