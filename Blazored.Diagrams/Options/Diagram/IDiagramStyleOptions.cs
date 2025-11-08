namespace Blazored.Diagrams.Options.Diagram;

/// <summary>
/// Default style options.
/// </summary>
public interface IDiagramStyleOptions
{
    /// <summary>
    /// Style of the diagram Container.
    /// </summary>
    string ContainerStyle { get; set; }

    /// <summary>
    /// Shows/Hides the grid.
    /// </summary>
    bool GridEnabled { get; set; }

    /// <summary>
    /// Size of the grid cells.
    /// </summary>
    int CellSize { get; set; }

    /// <summary>
    /// RGB color used for the grid lines. Default: <see cref="DiagramStyleOptions.DefaultGridLineColor"/>.
    /// </summary>
    string GridLineColor { get; set; }

    /// <summary>
    /// Opacity of the grid lines (0–1). Default: <see cref="DiagramStyleOptions.DefaultGridLineOpacity"/>.
    /// </summary>
    decimal GridLineOpacity { get; set; }

    /// <summary>
    /// Size of the grid lines in pixels. Default: <see cref="DiagramStyleOptions.DefaultGridLineThickness"/>.
    /// </summary>
    int GridLineThickness { get; set; }

    /// <summary>
    /// Gets or sets the grid style. If unset, falls back to <see cref="DiagramStyleOptions.DefaultGridStyle"/>.
    /// </summary>
    string GridStyle { get; set; }
}