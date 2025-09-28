namespace Blazored.Diagrams.Ports;

public interface IPortAnchor
{
    IPort Port { get; init; }
    int PositionX { get; set; }
    int PositionY { get; set; }
    
    /// <summary>
    /// Offset in pixels for the X position.
    /// </summary>
    public int OffsetX { get; set; }

    /// <summary>
    /// Offset in pixels for Y position.
    /// </summary>
    public int OffsetY { get; set; }
}