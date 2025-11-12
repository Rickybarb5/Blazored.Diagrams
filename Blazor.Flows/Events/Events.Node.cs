using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Flows.Events;

/// <summary>
/// Base class for a node event.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that triggered the event.</param>
public record NodeEvent(INode Model) : ModelEventBase<INode>(Model);

/// <summary>
/// Event triggered when a node is added to the diagram.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that was added.</param>
public record NodeAddedEvent(INode Model) : NodeEvent(Model);

/// <summary>
/// Event triggered when a node is removed from the diagram.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that was removed.</param>
public record NodeRemovedEvent(INode Model) : NodeEvent(Model);

/// <summary>
/// Event triggered when a node's position changes.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that moved.</param>
/// <param name="OldX">The old X position value.</param>
/// <param name="OldY">The old Y position value.</param>
/// <param name="NewX">The new X position value.</param>
/// <param name="NewY">The new Y position value.</param>
public record NodePositionChangedEvent(
    INode Model,
    double OldX,
    double OldY,
    double NewX,
    double NewY)
    : NodeEvent(Model);

/// <summary>
/// Event triggered when a node is selected or deselected.
/// </summary>
/// <param name="Model">The <see cref="INode"/> whose selection state changed.</param>
public record NodeSelectionChangedEvent(INode Model) : NodeEvent(Model);

/// <summary>
/// Event triggered when the visibility state of a node changes.
/// </summary>
/// <param name="Model">The <see cref="INode"/> whose visibility state changed.</param>
public record NodeVisibilityChangedEvent(INode Model) : NodeEvent(Model);

/// <summary>
/// Event triggered when the size (width or height) of a node changes.
/// </summary>
/// <param name="Model">The <see cref="INode"/> whose size changed.</param>
/// <param name="OldWidth">The previous width value.</param>
/// <param name="OldHeight">The previous height value.</param>
/// <param name="NewWidth">The new width value.</param>
/// <param name="NewHeight">The new height value.</param>
public record NodeSizeChangedEvent(INode Model, double OldWidth, double OldHeight, double NewWidth, double NewHeight)
    : NodeEvent(Model);

/// <summary>
/// Event triggered when a port is added to a node's collection.
/// </summary>
/// <param name="Node">The <see cref="INode"/> the port was added to.</param>
/// <param name="Port">The <see cref="IPort"/> that was added.</param>
public record PortAddedToNodeEvent(INode Node, IPort Port) : NodeEvent(Node);

/// <summary>
/// Event triggered when a port is removed from a node's collection.
/// </summary>
/// <param name="Model">The <see cref="INode"/> the port was removed from.</param>
/// <param name="Port">The <see cref="IPort"/> that was removed.</param>
public record PortRemovedFromNodeEvent(INode Model, IPort Port) : NodeEvent(Model);

/// <summary>
/// Event triggered when a pointer (e.g., mouse or touch) is pressed down on a node.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record NodePointerDownEvent(INode Model, PointerEventArgs Args) : ModelInputEvent<INode>(Model);

/// <summary>
/// Event triggered when a pointer is released from a node.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record NodePointerUpEvent(INode Model, PointerEventArgs Args) : ModelInputEvent<INode>(Model);

/// <summary>
/// Event triggered when a pointer moves over a node.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record NodePointerMoveEvent(INode Model, PointerEventArgs Args) : ModelInputEvent<INode>(Model);

/// <summary>
/// Event triggered when a pointer enters the bounds of a node.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record NodePointerEnterEvent(INode Model, PointerEventArgs Args) : ModelInputEvent<INode>(Model);

/// <summary>
/// Event triggered when a pointer leaves the bounds of a node.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record NodePointerLeaveEvent(INode Model, PointerEventArgs Args) : ModelInputEvent<INode>(Model);

/// <summary>
/// Event triggered when the mouse wheel is scrolled while over a node.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that triggered the event.</param>
/// <param name="Args">The <see cref="WheelEventArgs"/>.</param>
public record NodeWheelEvent(INode Model, WheelEventArgs Args) : ModelInputEvent<INode>(Model);

/// <summary>
/// Event triggered when an <c>@onclick</c> event occurs on a node.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that triggered the event.</param>
/// <param name="Args">The <see cref="MouseEventArgs"/>.</param>
public record NodeClickedEvent(INode Model, MouseEventArgs Args) : ModelInputEvent<INode>(Model);

/// <summary>
/// Event triggered when an <c>@ondblclick</c> event occurs on a node.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that triggered the event.</param>
/// <param name="Args">The <see cref="MouseEventArgs"/>.</param>
public record NodeDoubleClickedEvent(INode Model, MouseEventArgs Args) : ModelInputEvent<INode>(Model);

/// <summary>
/// Event triggered when a redraw is manually requested for a node.
/// </summary>
/// <param name="Model">The <see cref="INode"/> that requested the redraw.</param>
public record NodeRedrawEvent(INode Model) : NodeEvent(Model);

/// <summary>
/// Event triggered when <see cref="INode.ZIndex"/> changes;
/// </summary>
/// <param name="Model">The <see cref="INode"/> that triggered the event.</param>
public record NodeZIndexChanged(INode Model): NodeEvent(Model);