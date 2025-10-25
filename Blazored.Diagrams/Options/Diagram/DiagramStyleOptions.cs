using System.Globalization;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Options.Diagram;
/// <summary>
/// Style options for the diagram.
/// </summary>
public partial class DiagramStyleOptions : IDiagramStyleOptions
{
    readonly NumberFormatInfo _nfi = new();
    /// <summary>
    /// Default diagram container style.
    /// </summary>
    [JsonIgnore]
    public const string DefaultContainerStyle = "width:100%;height:100%;overflow:hidden;";

    /// <summary>
    /// Default cell size.
    /// </summary>
    [JsonIgnore]
    public const int DefaultCellSize = 20;

    /// <summary>
    /// Default grid line color in RGB values.
    /// </summary>
    [JsonIgnore]
    public const string DefaultGridLineColor = "0, 0, 0"; // RGB

    /// <summary>
    /// Default grid line opacity.
    /// </summary>
    private const decimal DefaultGridLineOpacity = 0.1m;
    
    /// <summary>
    /// Default grid line thickness in pixels.
    /// </summary>
    private const int DefaultGridLineThickness = 1;
    
    private int _cellSize;
    private string? _gridStyle;
    private string _gridLineColor;
    private decimal _gridLineOpacity;
    private int _gridLineThickness;

    /// <summary>
    /// Instantiates DiagramStyleOptions.
    /// </summary>
    public DiagramStyleOptions()
    {
        _nfi.NumberDecimalSeparator = ".";
        _nfi.NumberDecimalDigits = 3;
        _cellSize = DefaultCellSize;
        ContainerStyle = DefaultContainerStyle;
        _gridLineColor = DefaultGridLineColor;
        _gridLineOpacity = DefaultGridLineOpacity;
        _gridLineThickness = DefaultGridLineThickness;
        DefaultGridStyle = BuildDefaultGridStyle();
    }

    [JsonIgnore] 
    private string DefaultGridStyle { get; set; }

    /// <summary>
    /// Style of the diagram Container.
    /// </summary>
    public string ContainerStyle { get; set; }
    

    /// <summary>
    /// Shows/Hides the grid.
    /// </summary>
    public bool GridEnabled { get; set; } = true;

    /// <summary>
    /// Size of the grid cells.
    /// </summary>
    public int CellSize
    {
        get => _cellSize;
        set
        {
            _cellSize = value;
            DefaultGridStyle = BuildDefaultGridStyle();
        }
    }

    /// <summary>
    /// RGB color used for the grid lines. Default: <see cref="DefaultGridLineColor"/>.
    /// </summary>
    public string GridLineColor
    {
        get => _gridLineColor;
        set
        {
            _gridLineColor = value;
            DefaultGridStyle = BuildDefaultGridStyle();
        }
    }

    /// <summary>
    /// Opacity of the grid lines (0–1). Default: <see cref="DefaultGridLineOpacity"/>.
    /// </summary>
    public decimal GridLineOpacity
    {
        get => _gridLineOpacity;
        set
        {
            _gridLineOpacity = value;
            DefaultGridStyle = BuildDefaultGridStyle();
        }
    }

    /// <summary>
    /// Size of the grid lines in pixels. Default: <see cref="DefaultGridLineThickness"/>.
    /// </summary>
    public int GridLineThickness
    {
        get => _gridLineThickness;
        set
        {
            _gridLineThickness = value;
            DefaultGridStyle = BuildDefaultGridStyle();
        }
    }

    /// <summary>
    /// Gets or sets the grid style. If unset, falls back to <see cref="DefaultGridStyle"/>.
    /// </summary>
    public string GridStyle
    {
        get => string.IsNullOrWhiteSpace(_gridStyle) ? DefaultGridStyle : _gridStyle;
        set => _gridStyle = value;
    }

    private string BuildDefaultGridStyle()
    {
        return
            $"background-image:" +
            $"linear-gradient(to right, rgba({GridLineColor}, {GridLineOpacity.ToString(_nfi)}) {GridLineThickness}px, transparent {GridLineThickness}px)," +
            $"linear-gradient(to bottom, rgba({GridLineColor}, {GridLineOpacity.ToString(_nfi)}) {GridLineThickness}px, transparent {GridLineThickness}px);" +
            $"background-size:{CellSize}px {CellSize}px;";
    }
}