using Blazor.Flows.Groups;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Flows.Events;

// Group Events

/// <summary>
/// Base class for a group event.
/// </summary>
/// <param name="Model">The group that triggered the event.</param>
public record GroupEvent(IGroup Model) : ModelEventBase<IGroup>(Model);

/// <summary>
/// Event triggered when a group is added to the diagram.
/// </summary>
/// <param name="Model">The group that was added.</param>
public record GroupAddedEvent(IGroup Model) : GroupEvent(Model);

/// <summary>
/// Event triggered when a group is removed from the diagram.
/// </summary>
/// <param name="Model">The group that was removed.</param>
public record GroupRemovedEvent(IGroup Model) : GroupEvent(Model);

/// <summary>
/// Event triggered when a group position changes.
/// </summary>
/// <param name="Model">The group the moved.</param>
/// <param name="OldX">The old X position value.</param>
/// <param name="OldY">The old Y position value.</param>
/// <param name="NewX">The new X position value.</param>
/// <param name="NewY">The new Y position value.</param>
public record GroupPositionChangedEvent(IGroup Model, int OldX, int OldY, int NewX, int NewY)
    : GroupEvent(Model);

/// <summary>
/// Event triggered when the selection state of a group changes (e.g., selected or deselected).
/// </summary>
/// <param name="Model">The group whose selection state changed.</param>
public record GroupSelectionChangedEvent(IGroup Model) : GroupEvent(Model);

/// <summary>
/// Event triggered when the visibility state of a group changes.
/// </summary>
/// <param name="Model">The group whose visibility state changed.</param>
public record GroupVisibilityChangedEvent(IGroup Model) : GroupEvent(Model);

/// <summary>
/// Event triggered when the internal padding of a group changes.
/// </summary>
/// <param name="Model">The group whose padding changed.</param>
/// <param name="OldPadding">The old padding value in pixels.</param>
/// <param name="NewPadding">The new padding value in pixels.</param>
public record GroupPaddingChangedEvent(IGroup Model, int OldPadding, int NewPadding) : GroupEvent(Model);

/// <summary>
/// Event triggered when the size (width or height) of a group changes.
/// </summary>
/// <param name="Model">The group whose size changed.</param>
/// <param name="OldWidth">The previous width value in pixels.</param>
/// <param name="OldHeight">The previous height value in pixels.</param>
/// <param name="NewWidth">The new width value in pixels.</param>
/// <param name="NewHeight">The new height value in pixels.</param>
public record GroupSizeChangedEvent(IGroup Model, int OldWidth, int OldHeight, int NewWidth, int NewHeight)
    : GroupEvent(Model);

/// <summary>
/// Event triggered when a pointer (e.g., mouse or touch) is pressed down on a group.
/// </summary>
/// <param name="Model">The group that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record GroupPointerDownEvent(IGroup Model, PointerEventArgs Args) : ModelInputEvent<IGroup>(Model);

/// <summary>
/// Event triggered when a pointer is released from a group.
/// </summary>
/// <param name="Model">The group that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record GroupPointerUpEvent(IGroup Model, PointerEventArgs Args) : ModelInputEvent<IGroup>(Model);

/// <summary>
/// Event triggered when a pointer moves over a group.
/// </summary>
/// <param name="Model">The group that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record GroupPointerMoveEvent(IGroup Model, PointerEventArgs Args) : ModelInputEvent<IGroup>(Model);

/// <summary>
/// Event triggered when a pointer enters the bounds of a group.
/// </summary>
/// <param name="Model">The group that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record GroupPointerEnterEvent(IGroup Model, PointerEventArgs Args) : ModelInputEvent<IGroup>(Model);

/// <summary>
/// Event triggered when a pointer leaves the bounds of a group.
/// </summary>
/// <param name="Model">The group that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record GroupPointerLeaveEvent(IGroup Model, PointerEventArgs Args) : ModelInputEvent<IGroup>(Model);

/// <summary>
/// Event triggered when the mouse wheel is scrolled while over a group.
/// </summary>
/// <param name="Model">The group that triggered the event.</param>
/// <param name="Args">The <see cref="WheelEventArgs"/>.</param>
public record GroupWheelEvent(IGroup Model, WheelEventArgs Args) : ModelInputEvent<IGroup>(Model);

/// <summary>
/// Event triggered when an <c>@onclick</c> event occurs on a group.
/// </summary>
/// <param name="Model">The group that triggered the event.</param>
/// <param name="Args">The <see cref="MouseEventArgs"/>.</param>
public record GroupClickedEvent(IGroup Model, MouseEventArgs Args) : ModelInputEvent<IGroup>(Model);

/// <summary>
/// Event triggered when an <c>@ondblclick</c> event occurs on a group.
/// </summary>
/// <param name="Model">The group that triggered the event.</param>
/// <param name="Args">The <see cref="MouseEventArgs"/>.</param>
public record GroupDoubleClickedEvent(IGroup Model, MouseEventArgs Args) : ModelInputEvent<IGroup>(Model);

/// <summary>
/// Event triggered when a redraw is manually requested for a group.
/// </summary>
/// <param name="Model">The group that requested the redraw.</param>
public record GroupRedrawEvent(IGroup Model) : GroupEvent(Model);

/// <summary>
/// Event triggered when a port is added directly to a group's collection.
/// </summary>
/// <param name="Model">The group the port was added to.</param>
/// <param name="Port">The <see cref="IPort"/> that was added.</param>
public record PortAddedToGroupEvent(IGroup Model, IPort Port) : GroupEvent(Model);

/// <summary>
/// Event triggered when a port is removed from a group's collection.
/// </summary>
/// <param name="Model">The group the port was removed from.</param>
/// <param name="Port">The <see cref="IPort"/> that was removed.</param>
public record PortRemovedFromGroupEvent(IGroup Model, IPort Port) : GroupEvent(Model);

/// <summary>
/// Event triggered when a node is added to a group.
/// </summary>
/// <param name="Model">The group the node was added to.</param>
/// <param name="Node">The <see cref="INode"/> that was added.</param>
public record NodeAddedToGroupEvent(IGroup Model, INode Node) : GroupEvent(Model);

/// <summary>
/// Event triggered when a node is removed from a group.
/// </summary>
/// <param name="Model">The group the node was removed from.</param>
/// <param name="Node">The <see cref="INode"/> that was removed.</param>
public record NodeRemovedFromGroupEvent(IGroup Model, INode Node) : GroupEvent(Model);

/// <summary>
/// Event triggered when a group is added to another group.
/// </summary>
/// <param name="ParentModel">The parent group to which the child group was added.</param>
/// <param name="AddedGroup">The <see cref="IGroup"/> that was added.</param>
public record GroupAddedToGroupEvent(IGroup ParentModel, IGroup AddedGroup) : GroupEvent(ParentModel);

/// <summary>
/// Event triggered when a child group is removed from its parent group.
/// </summary>
/// <param name="ParentModel">The parent group from which the child group was removed.</param>
/// <param name="RemovedGroup">The <see cref="IGroup"/> that was removed.</param>
public record GroupRemovedFromGroupEvent(IGroup ParentModel, IGroup RemovedGroup) : GroupEvent(ParentModel);

/// <summary>
/// Event triggered when <see cref="IGroup.ZIndex"/> changes;
/// </summary>
/// <param name="Model">The group that triggered the event.</param>
public record GroupZIndexChangedEvent(IGroup Model): GroupEvent(Model);