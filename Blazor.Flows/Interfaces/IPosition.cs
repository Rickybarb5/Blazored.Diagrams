namespace Blazor.Flows.Interfaces;

/// <summary>
///     Properties that define the position of a model.
/// </summary>
public interface IPosition
{
    /// <summary>
    ///     Gets the position on the x-axis.
    /// </summary>
    public int PositionX { get; set; }

    /// <summary>
    ///     Gets the position on the y-axis.
    /// </summary>
    public int PositionY { get; set; }

    /// <summary>
    ///     Sets the X and Y coordinates on the screen.
    ///     Triggers the onPosition events.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void SetPosition(int x, int y);
}