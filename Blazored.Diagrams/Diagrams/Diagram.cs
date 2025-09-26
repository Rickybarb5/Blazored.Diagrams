using System.Text.Json.Serialization;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Diagrams;

/// <summary>
///     Base Diagram implementation.
/// </summary>
public partial class Diagram : IDiagram
{
    /// <inheritdoc />
    public virtual Guid Id { get; init; } = Guid.NewGuid();

    private ObservableList<ILayer> _layers =
    [
        //Default Layer
        new Layer { IsCurrentLayer = true },
    ];

    private int _panX;
    private int _panY;
    private double _zoom = 1;
    private int _positionX;
    private int _positionY;
    private int _width;
    private int _height;

    /// <summary>
    /// Instantiates a new <see cref="Diagram"/>
    /// </summary>
    public Diagram()
    {
        _layers.OnItemAdded += HandleLayerAdded;
        _layers.OnItemRemoved += HandleLayerRemoved;
    }

    private void HandleLayerRemoved(ILayer obj)
    {
        OnLayerRemoved?.Invoke(this, obj);
    }

    private void HandleLayerAdded(ILayer obj)
    {
        OnLayerAdded?.Invoke(this, obj);
    }

    /// <inheritdoc />
    public virtual DiagramOptions Options { get; set; } = new();

    /// <inheritdoc />
    public virtual double Zoom
    {
        get => _zoom;
        set => SetZoom(value);
    }

    /// <inheritdoc />
    public virtual int PanX
    {
        get => _panX;
        set
        {
            if (_panX != value)
            {
                var oldX = _panX;
                _panX = value;
                OnPanChanged?.Invoke(this, oldX, _panY, value, _panY);
            }
        }
    }

    /// <inheritdoc />
    public virtual int PanY
    {
        get => _panY;
        set
        {
            if (_panY != value)
            {
                var oldY = _panY;
                _panY = value;
                OnPanChanged?.Invoke(this, _panX, oldY, value, _panY);
            }
        }
    }


    /// <inheritdoc />
    [JsonIgnore]
    int IDiagram.Width
    {
        get => _width;
        set
        {
            if (value != _width)
            {
                var oldWidth = _width;
                _width = value;
                OnSizeChanged?.Invoke(this, oldWidth, _height, _width, _height);
            }
        }
    }

    /// <inheritdoc />
    [JsonIgnore]
    int IDiagram.Height
    {
        get => _width;
        set
        {
            if (value != _height)
            {
                var oldHeight = _height;
                _width = value;
                OnSizeChanged?.Invoke(this, _width, oldHeight, _width, _height);
            }
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<ILayer> Layers
    {
        get => _layers;
        init
        {
            _layers.Clear();
            value.ForEach(l=>_layers.Add(l));
        }
    }
    
    /// <inheritdoc />
    [JsonIgnore]
    public virtual IReadOnlyList<INode> AllNodes => _layers.SelectMany(layer => layer.AllNodes).ToList().AsReadOnly();

    /// <inheritdoc />
    [JsonIgnore]
    public virtual IReadOnlyList<ILink> AllLinks => _layers.SelectMany(layer => layer.AllLinks).ToList().AsReadOnly();

    /// <inheritdoc />
    [JsonIgnore]
    public virtual IReadOnlyList<IGroup> AllGroups =>
        _layers.SelectMany(layer => layer.AllGroups).ToList().AsReadOnly();

    /// <inheritdoc />
    [JsonIgnore]
    public virtual IReadOnlyList<IPort> AllPorts => _layers.SelectMany(layer => layer.AllPorts).ToList().AsReadOnly();

    /// <inheritdoc />
    public virtual ILayer CurrentLayer => Layers.First(l => l.IsCurrentLayer);


    /// <inheritdoc />
    [JsonIgnore]
    int IDiagram.PositionX
    {
        get => _positionX;
        set
        {
            if (value != _positionX)
            {
                var oldValue = _positionX;
                _positionX = value;
                OnPositionChanged?.Invoke(this, oldValue, _positionY, _positionX, _positionY);
            }
        }
    }

    /// <inheritdoc />
    [JsonIgnore]
    int IDiagram.PositionY
    {
        get => _positionY;
        set
        {
            if (value != _positionY)
            {
                var oldValue = _positionY;
                _positionY = value;
                OnPositionChanged?.Invoke(this, _positionX, oldValue, _positionX, _positionY);
            }
        }
    }

    /// <inheritdoc />
    public void Add(INode node)
    {
        CurrentLayer.Nodes.Add(node);
    }

    /// <inheritdoc />
    public void Add(IGroup group)
    {
        CurrentLayer.Groups.Add(group);
    }


    /// <inheritdoc />
    public event Action<IDiagram, double, double>? OnZoomChanged;

    /// <inheritdoc />
    public event Action<IDiagram, int, int, int, int>? OnPanChanged;

    /// <inheritdoc />
    public event Action<IDiagram, ILayer>? OnLayerAdded;

    /// <inheritdoc />
    public event Action<IDiagram, ILayer>? OnLayerRemoved;

    /// <inheritdoc />
    public event Action<IDiagram, int, int, int, int>? OnPositionChanged;

    /// <inheritdoc />
    public event Action<IDiagram, int, int, int, int>? OnSizeChanged;

    /// <inheritdoc />
    public void Dispose()
    {
        _layers.OnItemAdded -= HandleLayerAdded;
        _layers.OnItemRemoved -= HandleLayerRemoved;
    }
}