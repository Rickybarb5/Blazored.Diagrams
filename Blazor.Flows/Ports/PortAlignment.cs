using Blazor.Flows.Interfaces;

namespace Blazor.Flows.Ports;

/// <summary>
///     Defines the position of the port relative to the parent.
/// </summary>
public enum PortAlignment
{
    /// <summary>
    ///     Center of the port is on the left side of the parent position.
    /// </summary>
    Left,

    /// <summary>
    ///     Center of the port is on the right side of the parent position.
    /// </summary>
    Right,

    /// <summary>
    ///     Center of the port is on the top side of the parent position.
    /// </summary>
    Top,

    /// <summary>
    ///     Center of the port is on the bottom side of the parent position.
    /// </summary>
    Bottom,

    /// <summary>
    ///     Uses  <see cref="IPosition.PositionX" /> and <see cref="IPosition.PositionY" /> for the position. />
    /// </summary>
    Custom,

    /// <summary>
    /// Centers the port in the middle of the parent
    /// </summary>
    CenterParent
}