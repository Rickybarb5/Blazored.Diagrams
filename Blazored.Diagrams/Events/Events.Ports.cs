using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Ports;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Events;

// Port Events
public record PortEvent(IPort Model) : ModelEventBase<IPort>(Model);

public record PortAddedEvent(IPort Model) : PortEvent(Model);

public record PortRemovedEvent(IPort Model) : PortEvent(Model);

public record PortPositionChangedEvent(IPort Model, double OldX, double OldY, double NewX, double NewY)
    : PortEvent(Model);

public record PortAlignmentChangedEvent(IPort Model, PortAlignment OldPosition, PortAlignment NewPosition)
    : PortEvent(Model);

public record PortVisibilityChangedEvent(IPort Model) : PortEvent(Model);

public record PortParentChangedEvent(IPort Model, IPortContainer OldParent, IPortContainer NewParent)
    : PortEvent(Model);

public record IncomingLinkAddedEvent(IPort Model, ILink AddedLink) : PortEvent(Model);

public record IncomingLinkRemovedEvent(IPort Model, ILink RemovedLink) : PortEvent(Model);

public record OutgoingLinkAddedEvent(IPort Model, ILink AddedLink) : PortEvent(Model);

public record OutgoingLinkRemovedEvent(IPort Model, ILink RemovedLink) : PortEvent(Model);

public record PortSizeChangedEvent(IPort Model, double OldWidth, double OldHeight, double NewWidth, double NewHeight)
    : PortEvent(Model);

public record PortJustificationChangedEvent(
    IPort Model,
    PortJustification OldJustification,
    PortJustification NewJustification)
    : PortEvent(Model);

public record PortPointerDownEvent(IPort Model, PointerEventArgs Args) : ModelInputEvent<IPort>(Model);

public record PortPointerUpEvent(IPort Model, PointerEventArgs Args) : ModelInputEvent<IPort>(Model);

public record PortPointerMoveEvent(IPort Model, PointerEventArgs Args) : ModelInputEvent<IPort>(Model);

public record PortPointerEnterEvent(IPort Model, PointerEventArgs Args) : ModelInputEvent<IPort>(Model);

public record PortPointerLeaveEvent(IPort Model, PointerEventArgs Args) : ModelInputEvent<IPort>(Model);

public record PortWheelEvent(IPort Model, WheelEventArgs Args) : ModelInputEvent<IPort>(Model);

public record PortClickedEvent(IPort Model, MouseEventArgs Args) : ModelInputEvent<IPort>(Model);

public record PortDoubleClickedEvent(IPort Model, MouseEventArgs Args) : ModelInputEvent<IPort>(Model);

public record PortRedrawEvent(IPort Model) : PortEvent(Model);