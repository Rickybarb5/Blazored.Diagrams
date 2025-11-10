namespace Blazor.Flows.Interfaces;

/// <summary>
///     Interface to describe models that are zoomable.
/// </summary>
public interface IZoomable
{
    /// <summary>
    ///     Current zoom value
    /// </summary>
    public double Zoom { get; set; }
}