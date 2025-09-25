using Blazored.Diagrams.Links;
using Blazored.Diagrams.Ports;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Services.Events;

// Link Events
public record LinkEvent(ILink Model) : ModelEventBase<ILink>(Model);

public record LinkAddedEvent(ILink Model) : LinkEvent(Model);

public record LinkRemovedEvent(ILink Model) : LinkEvent(Model);

public record LinkSelectionChangedEvent(ILink Model) : LinkEvent(Model);

public record LinkVisibilityChangedEvent(ILink Model) : LinkEvent(Model);

public record LinkSizeChangedEvent(ILink Model, double OldWidth, double OldHeight, double NewWidth, double NewHeight)
    : LinkEvent(Model);

public record LinkTargetPositionChangedEvent(ILink Model, int OldX, int OldY, int TargetPositionX, int TargetPositionY)
    : LinkEvent(Model);

public record LinkSourcePortChangedEvent(ILink Model, IPort OldSourcePort, IPort NewSourcePort) : LinkEvent(Model);

public record LinkTargetPortChangedEvent(ILink Model, IPort? OldTargetPort, IPort? NewTargetPort) : LinkEvent(Model);

public record LinkPointerDownEvent(ILink Model, PointerEventArgs Args) : ModelInputEvent<ILink>(Model);

public record LinkPointerUpEvent(ILink Model, PointerEventArgs Args) : ModelInputEvent<ILink>(Model);

public record LinkPointerMoveEvent(ILink Model, PointerEventArgs Args) : ModelInputEvent<ILink>(Model);

public record LinkPointerEnterEvent(ILink Model, PointerEventArgs Args) : ModelInputEvent<ILink>(Model);

public record LinkPointerLeaveEvent(ILink Model, PointerEventArgs Args) : ModelInputEvent<ILink>(Model);

public record LinkWheelEvent(ILink Model, WheelEventArgs Args) : ModelInputEvent<ILink>(Model);

public record LinkClickedEvent(ILink Model, MouseEventArgs Args) : ModelInputEvent<ILink>(Model);

public record LinkDoubleClickedEvent(ILink Model, MouseEventArgs Args) : ModelInputEvent<ILink>(Model);

public record LinkRedrawEvent(ILink Model) : LinkEvent(Model);