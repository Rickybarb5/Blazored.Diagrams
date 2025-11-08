
using Blazored.Diagrams.Behaviours;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Options.Behaviours;

/// <summary>
/// Options for <see cref="ZoomBehavior"/>
/// </summary>
public class ZoomBehaviourOptions : BaseBehaviourOptions
{
    /// <summary>
    ///     Default value for  the zoom step.
    /// </summary>
    [JsonIgnore] private const double DefaultZoomStep = 0.1;

    /// <summary>
    ///     Default value for the minimum  zoom value.
    ///     Must be positive.
    /// </summary>
    [JsonIgnore] private const double DefaultMinimumZoom = 0.1;

    /// <summary>
    ///     Default value for the maximum zoom value.
    /// </summary>
    [JsonIgnore] private const double DefaultMaximumZoom = 3;

    /// <summary>
    ///     Minimum allowed zoom value of the diagram.
    /// </summary>
    public double MinZoom { get; set; } = DefaultMinimumZoom;

    /// <summary>
    ///     Maximum allowed zoom value of the diagram.
    /// </summary>
    public double MaxZoom { get; set; } = DefaultMaximumZoom;

    /// <summary>
    ///     Step value from zooming with the pointer wheel.
    /// </summary>
    public double ZoomStep { get; set; } = DefaultZoomStep;
}