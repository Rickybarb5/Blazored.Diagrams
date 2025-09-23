using Blazored.Diagrams.Components.Containers;
using Blazored.Diagrams.Diagrams;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Events;

/// <summary>
/// Mandatory interface to all record or classes that represent an event.
/// </summary>
public interface IEvent;

/// <summary>
/// Base class for a diagram event.
/// </summary>
/// <param name="Model"></param>
public record DiagramEvent(IDiagram Model) : ModelEventBase<IDiagram>(Model);

/// <summary>
/// Event triggered when the diagram position changes.
/// </summary>
/// <param name="Model">Diagram</param>
/// <param name="OldX">Old position x</param>
/// <param name="OldY">Old position Y</param>
/// <param name="NewX">New position X</param>
/// <param name="NewY">New position Y</param>
public record DiagramPositionChangedEvent(IDiagram Model, double OldX, double OldY, double NewX, double NewY)
    : DiagramEvent(Model);

/// <summary>
/// Event triggered when the diagram size changes.
/// </summary>
/// <param name="Model">Diagram.</param>
/// <param name="OldWidth">Previous width value.</param>
/// <param name="OldHeight">Previous height value. </param>
/// <param name="NewWidth">New diagram width</param>
/// <param name="NewHeight">New diagram height.</param>
public record DiagramSizeChangedEvent(
    IDiagram Model,
    double OldWidth,
    double OldHeight,
    double NewWidth,
    double NewHeight)
    : DiagramEvent(Model);

/// <summary>
/// Event triggered when the zoom changes.
/// </summary>
/// <param name="Model"></param>
/// <param name="OldZoom"></param>
/// <param name="NewZoom"></param>
public record DiagramZoomChangedEvent(IDiagram Model, double OldZoom, double NewZoom) : DiagramEvent(Model);

/// <summary>
/// Event triggered when the pan changes.
/// </summary>
/// <param name="Model"></param>
/// <param name="OldPanX"></param>
/// <param name="OldPanY"></param>
/// <param name="PanX"></param>
/// <param name="PanY"></param>
public record DiagramPanChangedEvent(IDiagram Model, int OldPanX, int OldPanY, int PanX, int PanY)
    : DiagramEvent(Model);

/// <summary>
/// Event triggered when a diagram redraw is requested.
/// </summary>
/// <param name="Model"></param>
public record DiagramRedrawEvent(IDiagram Model) : DiagramEvent(Model);

/// <summary>
///     Event triggered when an @onpointerdown event happens on the <see cref="DiagramContainer"/>
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">Pointer events.</param>
public record DiagramPointerDownEvent(IDiagram Model, PointerEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
///     Event triggered when an @onpointerup event happens on the <see cref="DiagramContainer"/>
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">Pointer events.</param>
public record DiagramPointerUpEvent(IDiagram Model, PointerEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
///     Event triggered when an @onpointermove event happens on the <see cref="DiagramContainer"/>
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">Pointer events.</param>
public record DiagramPointerMoveEvent(IDiagram Model, PointerEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
///     Event triggered when an @onpointerenter event happens on the <see cref="DiagramContainer"/>
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">Pointer events.</param>
public record DiagramPointerEnterEvent(IDiagram Model, PointerEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
///     Event triggered when an @onpointerleave event happens on the <see cref="DiagramContainer"/>
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">Pointer events.</param>
public record DiagramPointerLeaveEvent(IDiagram Model, PointerEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
///     Event triggered when an @onwheel event happens on the <see cref="DiagramContainer"/>
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args"> <see cref="WheelEventArgs"/>.</param>
public record DiagramWheelEvent(IDiagram Model, WheelEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
///     Event triggered when an @onclick event happens on the <see cref="DiagramContainer"/>
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args"><see cref="MouseEventArgs"/></param>
public record DiagramClickedEvent(IDiagram Model, MouseEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
///     Event triggered when an @ondblclick event happens on the <see cref="DiagramContainer"/>
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args"><see cref="MouseEventArgs"/></param>
public record DiagramDoubleClickedEvent(IDiagram Model, MouseEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
///     Event triggered when an @onkeyup event happens on the <see cref="DiagramContainer"/>
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args"><see cref="KeyboardEventArgs"/></param>
public record DiagramKeyUpEvent(IDiagram Model, KeyboardEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
///     Event triggered when an @onkeydown event happens on the <see cref="DiagramContainer"/>
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args"><see cref="KeyboardEventArgs"/></param>
public record DiagramKeyDownEvent(IDiagram Model, KeyboardEventArgs Args) : ModelInputEvent<IDiagram>(Model);