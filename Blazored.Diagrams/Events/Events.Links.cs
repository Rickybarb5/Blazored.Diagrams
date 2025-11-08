using Blazored.Diagrams.Links;
using Blazored.Diagrams.Ports;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Events;

// Link Events

/// <summary>
/// Base class for a link event.
/// </summary>
/// <param name="Model">The link that triggered the event.</param>
public record LinkEvent(ILink Model) : ModelEventBase<ILink>(Model);

/// <summary>
/// Event triggered when a link is added to the diagram.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> that was added.</param>
public record LinkAddedEvent(ILink Model) : LinkEvent(Model);

/// <summary>
/// Event triggered when a link is removed from the diagram.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> that was removed.</param>
public record LinkRemovedEvent(ILink Model) : LinkEvent(Model);

/// <summary>
/// Event triggered when the selection state of a link changes (e.g., selected or deselected).
/// </summary>
/// <param name="Model">The <see cref="ILink"/> whose selection state changed.</param>
public record LinkSelectionChangedEvent(ILink Model) : LinkEvent(Model);

/// <summary>
/// Event triggered when the visibility state of a link changes.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> whose visibility state changed.</param>
public record LinkVisibilityChangedEvent(ILink Model) : LinkEvent(Model);

/// <summary>
/// Event triggered when the size (width or height) of a link changes.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> whose size changed.</param>
/// <param name="OldWidth">The previous width value.</param>
/// <param name="OldHeight">The previous height value.</param>
/// <param name="NewWidth">The new width value.</param>
/// <param name="NewHeight">The new height value.</param>
public record LinkSizeChangedEvent(ILink Model, double OldWidth, double OldHeight, double NewWidth, double NewHeight)
    : LinkEvent(Model);

/// <summary>
/// Event triggered when the target position of a link changes. This is typically only used for in-progress or temporary links.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> whose target position changed.</param>
/// <param name="OldX">The old X position of the target point.</param>
/// <param name="OldY">The old Y position of the target point.</param>
/// <param name="TargetPositionX">The new X position of the target point.</param>
/// <param name="TargetPositionY">The new Y position of the target point.</param>
public record LinkTargetPositionChangedEvent(ILink Model, int OldX, int OldY, int TargetPositionX, int TargetPositionY)
    : LinkEvent(Model);

/// <summary>
/// Event triggered when the source port of a link is changed to a different port.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> whose source port changed.</param>
/// <param name="OldSourcePort">The previous <see cref="IPort"/> the link was connected to.</param>
/// <param name="NewSourcePort">The new <see cref="IPort"/> the link is now connected to.</param>
public record LinkSourcePortChangedEvent(ILink Model, IPort? OldSourcePort, IPort NewSourcePort) : LinkEvent(Model);

/// <summary>
/// Event triggered when the target port of a link is changed.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> whose target port changed.</param>
/// <param name="OldTargetPort">The previous <see cref="IPort"/> the link was connected to (can be null).</param>
/// <param name="NewTargetPort">The new <see cref="IPort"/> the link is now connected to (can be null).</param>
public record LinkTargetPortChangedEvent(ILink Model, IPort? OldTargetPort, IPort? NewTargetPort) : LinkEvent(Model);

/// <summary>
/// Event triggered when a pointer (e.g., mouse or touch) is pressed down on a link.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record LinkPointerDownEvent(ILink Model, PointerEventArgs Args) : ModelInputEvent<ILink>(Model);

/// <summary>
/// Event triggered when a pointer is released from a link.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record LinkPointerUpEvent(ILink Model, PointerEventArgs Args) : ModelInputEvent<ILink>(Model);

/// <summary>
/// Event triggered when a pointer moves over a link.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record LinkPointerMoveEvent(ILink Model, PointerEventArgs Args) : ModelInputEvent<ILink>(Model);

/// <summary>
/// Event triggered when a pointer enters the bounds of a link.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record LinkPointerEnterEvent(ILink Model, PointerEventArgs Args) : ModelInputEvent<ILink>(Model);

/// <summary>
/// Event triggered when a pointer leaves the bounds of a link.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record LinkPointerLeaveEvent(ILink Model, PointerEventArgs Args) : ModelInputEvent<ILink>(Model);

/// <summary>
/// Event triggered when the mouse wheel is scrolled while over a link.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> that triggered the event.</param>
/// <param name="Args">The <see cref="WheelEventArgs"/>.</param>
public record LinkWheelEvent(ILink Model, WheelEventArgs Args) : ModelInputEvent<ILink>(Model);

/// <summary>
/// Event triggered when an <c>@onclick</c> event occurs on a link.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> that triggered the event.</param>
/// <param name="Args">The <see cref="MouseEventArgs"/>.</param>
public record LinkClickedEvent(ILink Model, MouseEventArgs Args) : ModelInputEvent<ILink>(Model);

/// <summary>
/// Event triggered when an <c>@ondblclick</c> event occurs on a link.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> that triggered the event.</param>
/// <param name="Args">The <see cref="MouseEventArgs"/>.</param>
public record LinkDoubleClickedEvent(ILink Model, MouseEventArgs Args) : ModelInputEvent<ILink>(Model);

/// <summary>
/// Event triggered when a redraw is manually requested for a link.
/// </summary>
/// <param name="Model">The <see cref="ILink"/> that requested the redraw.</param>
public record LinkRedrawEvent(ILink Model) : LinkEvent(Model);