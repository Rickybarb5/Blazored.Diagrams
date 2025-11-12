using Blazor.Flows.Events;
using Blazor.Flows.Interfaces;
using Newtonsoft.Json;

namespace Blazor.Flows.Ports;

/// <summary>
///     A port is what allows a model to connect/be connected to another model.
/// </summary>
public interface IPort :
    IId,
    IDepth,
    IVisible,
    ISize,
    IPosition,
    ILinkContainer,
    ISelectable,
    IDisposable
{
    /// <summary>
    /// Offset of the port position on the X axis in pixels.
    /// </summary>
    int OffSetX { get; set; }

    /// <summary>
    /// Offset of the port position on the Y axis in pixels.
    /// </summary>
    int OffsetY { get; set; }

    /// <summary>
    ///     Where the port will be aligned
    /// </summary>
    PortJustification Justification { get; set; }


    /// <summary>
    /// Where the port will be placed.
    /// </summary>
    PortAlignment Alignment { get; set; }


    /// <summary>
    ///     Parent of the port.
    /// </summary>
    IPortContainer Parent { get; set; }

    /// <summary>
    ///     Indicates if the port has one or more links attached.
    /// </summary>
    bool HasLinks { get; }

    /// <summary>
    /// True if the port has incoming links, false otherwise.
    /// </summary>
    bool HasIncomingLinks { get; }

    /// <summary>
    /// True if the port has outgoing links, false otherwise.
    /// </summary>
    bool HasOutGoingLinks { get; }

    /// <summary>
    ///     Event triggered when the port alignment changes.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<PortJustificationChangedEvent> OnPortJustificationChanged { get; init; }

    /// <summary>
    ///     Event triggered when PortPosition changes.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<PortAlignmentChangedEvent> OnPortAlignmentChanged { get; init; }

    /// <summary>
    ///     Event triggered when the position is changed.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<PortPositionChangedEvent> OnPositionChanged { get; init; }

    /// <summary>
    ///     Event triggered when the size changes.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<PortSizeChangedEvent> OnSizeChanged { get; init; }

    /// <summary>
    /// Event triggered when the port is assigned to a container.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<PortParentChangedEvent> OnPortParentChanged { get; init; }

    /// <summary>
    ///     Event triggered when the <see cref="IVisible.IsVisible" /> flag is changed.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<PortVisibilityChangedEvent> OnVisibilityChanged { get; init; }

    /// <summary>
    ///     Event triggered when a incoming link is attached to the port.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<IncomingLinkAddedEvent> OnIncomingLinkAdded { get; init; }

    /// <summary>
    ///     Event triggered when a incoming link is detached to the port.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<IncomingLinkRemovedEvent> OnIncomingLinkRemoved { get; init; }

    /// <summary>
    ///     Event triggered when a outgoing link is attached to the port.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<OutgoingLinkAddedEvent> OnOutgoingLinkAdded { get; init; }

    /// <summary>
    ///     Event triggered when a outgoing link is detached to the port.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<OutgoingLinkRemovedEvent> OnOutgoingLinkRemoved { get; init; }

    /// <summary>
    ///     Event triggered when a link removed from the port.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<LinkRemovedEvent> OnLinkRemoved { get; init; }

    /// <summary>
    ///     Event triggered when a link is added the port.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<LinkAddedEvent> OnLinkAdded { get; init; }

    /// <summary>
    /// Event triggered when a port is selected/unselected.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<PortSelectionChangedEvent> OnSelectionChanged { get; init; }
    
    /// <summary>
    /// Event triggered when the <see cref="IPort.ZIndex"/> changes.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<PortZIndexChanged> OnZIndexChanged { get; init; }

    /// <summary>
    /// Indicates if link creation is possible from this port.
    /// </summary>
    bool CanCreateLink();
    
    /// <summary>
    ///     Indicates if this port can connect to another port.
    /// </summary>
    /// <param name="targetPort">Port to evaluate if connection is possible</param>
    /// <returns>True if the link can connect to the input port, false otherwise.</returns>
    bool CanConnectTo(IPort targetPort);

    /// <summary>
    ///     Sets the X and Y coordinates on the screen.
    ///     Does not trigger the event.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void SetPositionInternal(int x, int y);

    /// <summary>
    ///     Sets the width and height.
    ///     Does not trigger the event.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    void SetSizeInternal(int width, int height);

    /// <summary>
    /// Custom function that is used when <see cref="PortAlignment"/> is <see cref="PortAlignment.Custom"/>.
    /// </summary>
    /// <returns>The calculated X and Y positions.</returns>
    public (int PositionX, int PositionY) CustomPositioning();
}