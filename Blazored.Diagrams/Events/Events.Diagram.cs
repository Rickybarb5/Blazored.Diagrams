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
/// <param name="Model">The diagram that triggered the event.</param>
public record DiagramEvent(IDiagram Model) : ModelEventBase<IDiagram>(Model);

/// <summary>
/// Event triggered when the diagram position changes.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="OldX">The previous X position value.</param>
/// <param name="OldY">The previous Y position value.</param>
/// <param name="NewX">The new X position value.</param>
/// <param name="NewY">The new Y position value.</param>
public record DiagramPositionChangedEvent(IDiagram Model, double OldX, double OldY, double NewX, double NewY)
    : DiagramEvent(Model);

/// <summary>
/// Event triggered when the diagram size changes.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="OldWidth">Previous width value in pixels.</param>
/// <param name="OldHeight">Previous height value in pixels. </param>
/// <param name="NewWidth">New diagram width in pixels.</param>
/// <param name="NewHeight">New diagram height in pixels.</param>
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
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="OldZoom">The previous zoom factor (e.g., 1.0 for 100%).</param>
/// <param name="NewZoom">The new zoom factor.</param>
public record DiagramZoomChangedEvent(IDiagram Model, double OldZoom, double NewZoom) : DiagramEvent(Model);

/// <summary>
/// Event triggered when the pan changes.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="OldPanX">The previous pan X offset value in pixels.</param>
/// <param name="OldPanY">The previous pan Y offset value in pixels.</param>
/// <param name="PanX">The new pan X offset value in pixels.</param>
/// <param name="PanY">The new pan Y offset value in pixels.</param>
public record DiagramPanChangedEvent(IDiagram Model, int OldPanX, int OldPanY, int PanX, int PanY)
    : DiagramEvent(Model);

/// <summary>
/// Event triggered when a diagram redraw is requested.
/// </summary>
/// <param name="Model">The diagram that requested the redraw.</param>
public record DiagramRedrawEvent(IDiagram Model) : DiagramEvent(Model);

/// <summary>
/// Event triggered when an <c>@onpointerdown</c> event happens on the <see cref="DiagramContainer"/>.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record DiagramPointerDownEvent(IDiagram Model, PointerEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
/// Event triggered when an <c>@onpointerup</c> event happens on the <see cref="DiagramContainer"/>.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record DiagramPointerUpEvent(IDiagram Model, PointerEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
/// Event triggered when an <c>@onpointermove</c> event happens on the <see cref="DiagramContainer"/>.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record DiagramPointerMoveEvent(IDiagram Model, PointerEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
/// Event triggered when an <c>@onpointerenter</c> event happens on the <see cref="DiagramContainer"/>.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record DiagramPointerEnterEvent(IDiagram Model, PointerEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
/// Event triggered when an <c>@onpointerleave</c> event happens on the <see cref="DiagramContainer"/>.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">The <see cref="PointerEventArgs"/>.</param>
public record DiagramPointerLeaveEvent(IDiagram Model, PointerEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
/// Event triggered when an <c>@onwheel</c> event happens on the <see cref="DiagramContainer"/>.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">The <see cref="WheelEventArgs"/>.</param>
public record DiagramWheelEvent(IDiagram Model, WheelEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
/// Event triggered when an <c>@onclick</c> event happens on the <see cref="DiagramContainer"/>.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">The <see cref="MouseEventArgs"/>.</param>
public record DiagramClickedEvent(IDiagram Model, MouseEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
/// Event triggered when an <c>@ondblclick</c> event happens on the <see cref="DiagramContainer"/>.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">The <see cref="MouseEventArgs"/>.</param>
public record DiagramDoubleClickedEvent(IDiagram Model, MouseEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
/// Event triggered when an <c>@onkeyup</c> event happens on the <see cref="DiagramContainer"/>.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">The <see cref="KeyboardEventArgs"/>.</param>
public record DiagramKeyUpEvent(IDiagram Model, KeyboardEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
/// Event triggered when an <c>@onkeydown</c> event happens on the <see cref="DiagramContainer"/>.
/// </summary>
/// <param name="Model">The diagram that triggered the event.</param>
/// <param name="Args">The <see cref="KeyboardEventArgs"/>.</param>
public record DiagramKeyDownEvent(IDiagram Model, KeyboardEventArgs Args) : ModelInputEvent<IDiagram>(Model);

/// <summary>
/// Event triggered when the diagram instance is replaced with another.
/// </summary>
/// <param name="OldDiagram">The diagram that was replaced.</param>
/// <param name="NewDiagram">The current diagram in the diagram service.</param>
public record DiagramSwitchEvent(IDiagram OldDiagram, IDiagram NewDiagram):IEvent;