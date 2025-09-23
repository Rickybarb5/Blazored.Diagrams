using System.Text.Json.Serialization;

namespace Blazored.Diagrams.Options.Diagram;

/// <summary>
///     Style options for the diagram.
/// </summary>
public class DiagramStyleOptions
{
    /// <summary>
    /// Gets the default style for the diagram container.
    /// </summary>
    public static string DefaultDiagramContainerStyle { get; } =
        "width:100%;height:100%;overflow:hidden;";

    private int _cellSize = 20;

    private string _gridStyle = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public DiagramStyleOptions()
    {
        SetDefaultGridStyle(_cellSize);
    }

    /// <summary>
    /// Gets the default grid style.
    /// </summary>
    [JsonIgnore]
    public string DefaultGridStyle { get; private set; }

    /// <summary>
    /// Gets or sets the container style.
    /// </summary>
    public string ContainerStyle { get; set; } = DefaultDiagramContainerStyle;

    /// <summary>
    /// Enables drawing of the grid.
    /// </summary>
    public bool GridEnabled { get; set; } = true;

    /// <summary>
    /// Size of the Grid cells.
    /// </summary>
    public int CellSize
    {
        get => _cellSize;
        set
        {
            _cellSize = value;
            SetDefaultGridStyle(_cellSize);
        }
    }

    /// <summary>
    /// Gets or sets the grid style.
    /// </summary>
    public string GridStyle
    {
        get => string.IsNullOrWhiteSpace(_gridStyle) ? DefaultGridStyle : _gridStyle;
        set => _gridStyle = value;
    }

    private void SetDefaultGridStyle(int cellSize)
    {
        DefaultGridStyle =
            $"background-image:linear-gradient(to right, rgba(0, 0, 0, 0.1) 1px, transparent 1px),linear-gradient(to bottom, rgba(0, 0, 0, 0.1) 1px, transparent 1px); background-size:{cellSize}px {cellSize}px;";
    }
}