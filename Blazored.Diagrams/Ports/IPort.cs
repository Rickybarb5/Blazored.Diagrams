using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Links;

namespace Blazored.Diagrams.Ports;

/// <summary>
///     A port is what allows a model to connect/be connected to another model.
/// </summary>
public interface IPort :
    IId,
    IVisible,
    IPosition,
    ISize,
    ILinkContainer,
    IDisposable
{
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
    /// Indicates if link creation is possible from this port.
    /// </summary>
    bool CanCreateLink();


    /// <summary>
    ///     Indicates if this port can connect to another port.
    /// </summary>
    /// <param name="port">Port to evaluate if connection is possible</param>
    /// <returns>True if the link can connect to the input port, false otherwise.</returns>
    bool CanConnectTo(IPort port);

    //TODO:Add an allows connection or something.

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
    ///     Event triggered when the port alignment changes.
    /// </summary>
    public event Action<IPort, PortJustification, PortJustification> OnPortJustificationChanged;

    /// <summary>
    ///     Event triggered when PortPosition changes.
    /// </summary>
    public event Action<IPort, PortAlignment, PortAlignment> OnPortAlignmentChanged;

    /// <summary>
    ///     Event triggered when the position is changed.
    /// </summary>
    event Action<IPort, int, int, int, int> OnPositionChanged;

    /// <summary>
    ///     Event triggered when the size changes.
    /// </summary>
    public event Action<IPort, int, int, int, int> OnSizeChanged;

    /// <summary>
    /// Event triggered when the port is assigned to a container.
    /// </summary>
    public event Action<IPort, IPortContainer, IPortContainer>? OnPortParentChanged;

    /// <summary>
    ///     Event triggered when the <see cref="IVisible.IsVisible" /> flag is changed.
    /// </summary>
    event Action<IPort> OnVisibilityChanged;

    /// <summary>
    ///     Event triggered when a incoming link is attached to the port.
    /// </summary>
    public event Action<IPort, ILink> OnIncomingLinkAdded;

    /// <summary>
    ///     Event triggered when a incoming link is detached to the port.
    /// </summary>
    public event Action<IPort, ILink> OnIncomingLinkRemoved;

    /// <summary>
    ///     Event triggered when a outgoing link is attached to the port.
    /// </summary>
    public event Action<IPort, ILink> OnOutgoingLinkAdded;

    /// <summary>
    ///     Event triggered when a outgoing link is detached to the port.
    /// </summary>
    public event Action<IPort, ILink> OnOutgoingLinkRemoved;

    /// <summary>
    ///     Calculates the x and y coordinates based on the position and alignment.
    /// </summary>
    /// <returns>The x and y coordinates</returns>
    (int PositionX, int PositionY) CalculatePosition();

    /// <summary>
    /// Recalculates the port position
    /// </summary>
    void RefreshPositionCoordinates();
}