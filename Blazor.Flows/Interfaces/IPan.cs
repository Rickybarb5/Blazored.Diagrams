namespace Blazor.Flows.Interfaces;

/// <summary>
///     Describes the panning properties for the diagram.
/// </summary>
public interface IPan
{
    /// <summary>
    ///     X coordinate of the Pan.
    /// </summary>
    public int PanX { get; set; }

    /// <summary>
    ///     Y coordinate of the Pan.
    /// </summary>
    public int PanY { get; set; }

    /// <summary>
    ///     Sets the X and Y coordinates of the Pan.
    ///     Triggers the OnPan events.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetPan(int x, int y);
}