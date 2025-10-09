using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Services.Events;

/// <summary>
///     Base layer event.
/// </summary>
/// <param name="Model">Node that triggered the event</param>
public record NodeEvent(INode Model) : ModelEventBase<INode>(Model);

/// <summary>
///     Event triggered when a node is added to the diagram.
/// </summary>
/// <param name="Model"></param>
public record NodeAddedEvent(INode Model) : NodeEvent(Model);

/// <summary>
///     Event triggered when a node is removed from the diagram.
/// </summary>
/// <param name="Model"></param>
public record NodeRemovedEvent(INode Model) : NodeEvent(Model);

/// <summary>
///     Event triggered when a node's position changes.
/// </summary>
/// <param name="Model"></param>
/// <param name="OldWidth"></param>
/// <param name="OldHeight"></param>
/// <param name="NewWidth"></param>
/// <param name="NewHeight"></param>
public record NodePositionChangedEvent(
    INode Model,
    double OldWidth,
    double OldHeight,
    double NewWidth,
    double NewHeight)
    : NodeEvent(Model);

/// <summary>
///     Event triggered when a node is selected/deselected.
/// </summary>
/// <param name="Model"></param>
public record NodeSelectionChangedEvent(INode Model) : NodeEvent(Model);

/// <summary>
///     Event triggered when the visibility of a node changes.
/// </summary>
/// <param name="Model"></param>
public record NodeVisibilityChangedEvent(INode Model) : NodeEvent(Model);

/// <summary>
/// Event triggered when the size of a node changes.
/// </summary>
/// <param name="Model"></param>
/// <param name="OldWidth"></param>
/// <param name="OldHeight"></param>
/// <param name="NewWidth"></param>
/// <param name="NewHeight"></param>
public record NodeSizeChangedEvent(INode Model, double OldWidth, double OldHeight, double NewWidth, double NewHeight)
    : NodeEvent(Model);

/// <summary>
///     Event triggered when a port is added to a node.
/// </summary>
/// <param name="Node"></param>
/// <param name="Port"></param>
public record PortAddedToNodeEvent(INode Node, IPort Port) : NodeEvent(Node);

/// <summary>
///     Event triggered when a port is removed from a node.
/// </summary>
/// <param name="Model"></param>
/// <param name="Port"></param>
public record PortRemovedFromNodeEvent(INode Model, IPort Port) : NodeEvent(Model);

public record NodePointerDownEvent(INode Model, PointerEventArgs Args) : ModelInputEvent<INode>(Model);

public record NodePointerUpEvent(INode Model, PointerEventArgs Args) : ModelInputEvent<INode>(Model);

public record NodePointerMoveEvent(INode Model, PointerEventArgs Args) : ModelInputEvent<INode>(Model);

public record NodePointerEnterEvent(INode Model, PointerEventArgs Args) : ModelInputEvent<INode>(Model);

public record NodePointerLeaveEvent(INode Model, PointerEventArgs Args) : ModelInputEvent<INode>(Model);

public record NodeWheelEvent(INode Model, WheelEventArgs Args) : ModelInputEvent<INode>(Model);

public record NodeClickedEvent(INode Model, MouseEventArgs Args) : ModelInputEvent<INode>(Model);

public record NodeDoubleClickedEvent(INode Model, MouseEventArgs Args) : ModelInputEvent<INode>(Model);

public record NodeRedrawEvent(INode Model) : NodeEvent(Model);