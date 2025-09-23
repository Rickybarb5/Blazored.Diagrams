namespace Blazored.Diagrams.Interfaces;

/// <summary>
///     Interface to describe models that are zoomable.
/// </summary>
public interface IZoomable
{
    /// <summary>
    ///     Current zoom value
    /// </summary>
    public double Zoom { get; set; }

    /// <summary>
    ///     Sets the zoom to the specified value.
    /// </summary>
    /// <param name="zoom"></param>
    public void SetZoom(double zoom);
}