using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Diagram;
using Blazored.Diagrams.Ports;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Diagrams;

/// <summary>
///     Base Diagram implementation.
/// </summary>
public partial class Diagram : IDiagram
{
    /// <inheritdoc />
    public virtual string Id { get; init; } = Guid.NewGuid().ToString();

    private ObservableList<ILayer> _layers = [];

    private int _panX;
    private int _panY;
    private decimal _zoom = 1;
    private int _positionX;
    private int _positionY;
    private int _width;
    private int _height;
    private ILayer _currentLayer;

    /// <summary>
    /// Instantiates a new <see cref="Diagram"/>
    /// </summary>
    public Diagram()
    {
        _layers.OnItemAdded.Subscribe(HandleLayerAdded);
        _layers.OnItemRemoved.Subscribe(HandleLayerRemoved);
        // Always ensure a default layer exists
        EnsureDefaultLayer();
    }
    
    private void EnsureDefaultLayer()
    {
        if (_layers.All(l=>l.Id != Guid.Empty.ToString()))
        {
            var defaultLayer = new Layer
            {
                Id = Guid.Empty.ToString(),
            };
            _layers.AddInternal(defaultLayer);
            _currentLayer = defaultLayer;
        }
    }

    private void HandleLayerRemoved(ItemRemovedEvent<ILayer> ev)
    {
        OnLayerRemoved.Publish(new(ev.Item));
    }

    private void HandleLayerAdded(ItemAddedEvent<ILayer> ev)
    {
        OnLayerAdded.Publish(new(ev.Item));
    }

    /// <inheritdoc />
    public virtual decimal Zoom
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
                OnPanChanged.Publish(new(this, oldX, _panY, value, _panY));
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
                OnPanChanged.Publish(new(this, _panX, oldY, value, _panY));
            }
        }
    }


    /// <inheritdoc />
    [JsonIgnore]
    public int Width
    {
        get => _width;
        set
        {
            if (value != _width)
            {
                var oldWidth = _width;
                _width = value;
                OnSizeChanged.Publish(new(this, oldWidth, _height, _width, _height));
            }
        }
    }

    /// <inheritdoc />
    [JsonIgnore]
    public int Height
    {
        get => _height;
        set
        {
            if (value != _height)
            {
                var oldHeight = _height;
                _height = value;
                OnSizeChanged.Publish(new(this, _width, oldHeight, _width, _height));
            }
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<ILayer> Layers
    {
        get => _layers;
        set
        {
            _layers.ClearInternal();
            value.ForEach(l=> _layers.AddInternal(l));
            EnsureDefaultLayer();
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
    [JsonIgnore]
    public IReadOnlyList<ISelectable> SelectedModels =>
        Layers.SelectMany(l => l.SelectedModels).ToList().AsReadOnly();
    /// <inheritdoc />
    public virtual ILayer CurrentLayer
    {
        get => _currentLayer;
        set
        {
            if (_currentLayer != value)
            {
                // add layer if it doesn't exist
                if (_layers.FirstOrDefault(l => l.Id == value.Id) is null)
                {
                    _layers.AddInternal(value);
                }

                var oldLayer = _currentLayer;
                _currentLayer = value;
                OnCurrentLayerChanged.Publish(new(oldLayer, _currentLayer));
            }
        }
    }

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
                OnPositionChanged.Publish(new(this, oldValue, _positionY, _positionX, _positionY));
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
                OnPositionChanged.Publish(new(this, _positionX, oldValue, _positionX, _positionY));
            }
        }
    }

    /// <inheritdoc />
    public virtual IDiagramOptions Options { get; init; } = new DiagramOptions();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<DiagramSizeChangedEvent> OnSizeChanged { get; init; } =
        new TypedEvent<DiagramSizeChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<DiagramZoomChangedEvent> OnZoomChanged { get; init; } =
        new TypedEvent<DiagramZoomChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<DiagramPanChangedEvent> OnPanChanged { get; init; } = new TypedEvent<DiagramPanChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<LayerAddedEvent> OnLayerAdded { get; init; } = new TypedEvent<LayerAddedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<LayerRemovedEvent> OnLayerRemoved { get; init; } = new TypedEvent<LayerRemovedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<DiagramPositionChangedEvent> OnPositionChanged { get; init; } =
        new TypedEvent<DiagramPositionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<CurrentLayerChangedEvent>? OnCurrentLayerChanged { get; init; } =
        new TypedEvent<CurrentLayerChangedEvent>();


    /// <inheritdoc />
    public void Dispose()
    {
        _layers.OnItemAdded.Unsubscribe(HandleLayerAdded);
        _layers.OnItemRemoved.Unsubscribe(HandleLayerRemoved);
    }
}