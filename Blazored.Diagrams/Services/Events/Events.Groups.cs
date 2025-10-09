using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Services.Events;

// Group Events
public record GroupEvent(IGroup Model) : ModelEventBase<IGroup>(Model);

public record GroupAddedEvent(IGroup Model) : GroupEvent(Model);

public record GroupRemovedEvent(IGroup Model) : GroupEvent(Model);

public record GroupPositionChangedEvent(IGroup Model, int OldX, int OldY, int NewX, int NewY)
    : GroupEvent(Model);

public record GroupSelectionChangedEvent(IGroup Model) : GroupEvent(Model);

public record GroupVisibilityChangedEvent(IGroup Model) : GroupEvent(Model);

public record GroupPaddingChangedEvent(IGroup Model, int OldPadding, int NewPadding) : GroupEvent(Model);

public record GroupSizeChangedEvent(IGroup Model, int OldWidth, int OldHeight, int NewWidth, int NewHeight)
    : GroupEvent(Model);

public record GroupPointerDownEvent(IGroup Model, PointerEventArgs Args) : ModelInputEvent<IGroup>(Model);

public record GroupPointerUpEvent(IGroup Model, PointerEventArgs Args) : ModelInputEvent<IGroup>(Model);

public record GroupPointerMoveEvent(IGroup Model, PointerEventArgs Args) : ModelInputEvent<IGroup>(Model);

public record GroupPointerEnterEvent(IGroup Model, PointerEventArgs Args) : ModelInputEvent<IGroup>(Model);

public record GroupPointerLeaveEvent(IGroup Model, PointerEventArgs Args) : ModelInputEvent<IGroup>(Model);

public record GroupWheelEvent(IGroup Model, WheelEventArgs Args) : ModelInputEvent<IGroup>(Model);

public record GroupClickedEvent(IGroup Model, MouseEventArgs Args) : ModelInputEvent<IGroup>(Model);

public record GroupDoubleClickedEvent(IGroup Model, MouseEventArgs Args) : ModelInputEvent<IGroup>(Model);

public record GroupRedrawEvent(IGroup Model) : GroupEvent(Model);

public record PortAddedToGroupEvent(IGroup Model, IPort Port) : GroupEvent(Model);

public record PortRemovedFromGroupEvent(IGroup Model, IPort Port) : GroupEvent(Model);

/// <summary>
///     Event triggered when a node is added to a group.
/// </summary>
/// <param name="Model"></param>
/// <param name="Node"></param>
public record NodeAddedToGroupEvent(IGroup Model, INode Node) : GroupEvent(Model);

public record NodeRemovedFromGroupEvent(IGroup Model, INode Node) : GroupEvent(Model);

/// <summary>
/// Event triggered when a group is added to another group.
/// </summary>
/// <param name="ParentModel"></param>
/// <param name="AddedGroup"></param>
public record GroupAddedToGroupEvent(IGroup ParentModel, IGroup AddedGroup) : GroupEvent(ParentModel);

public record GroupRemovedFromGroupEvent(IGroup ParentModel, IGroup RemovedGroup) : GroupEvent(ParentModel);