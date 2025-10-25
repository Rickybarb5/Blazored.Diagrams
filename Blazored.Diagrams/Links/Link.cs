using Blazored.Diagrams.Components.Models;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Registry;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Links;

/// <summary>
/// Default link implementation.
/// </summary>
public abstract partial class Link : ILink, IHasComponent<DefaultLinkComponent>
{
    private bool _isSelected;
    private bool _isVisible = true;
    private IPort _sourcePort;
    private IPort? _targetPort;
    private int _targetPositionX;
    private int _targetPositionY;
    private int _width;
    private int _height;

    /// <inheritdoc />
    public virtual string Id { get; init; } = Guid.NewGuid().ToString();

    /// <inheritdoc />
    /// TODO:Check if required feels good.
    [JsonIgnore]
    public virtual IPort SourcePort
    {
        get => _sourcePort;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (_sourcePort != value)
            {
                var oldPort = _sourcePort;
                _sourcePort = value;
                _sourcePort.OutgoingLinks.Add(this);
                OnSourcePortChanged.Publish(new(this, oldPort, _sourcePort));
            }
        }
    }

    /// <inheritdoc />
    [JsonIgnore]
    public virtual IPort? TargetPort
    {
        get => _targetPort;
        set
        {
            if (_targetPort != value)
            {
                var oldTargetPort = _targetPort;
                _targetPort = value;
                _targetPort?.IncomingLinks.Add(this);
                OnTargetPortChanged.Publish(new(this, oldTargetPort, _targetPort));
            }
        }
    }

    /// <inheritdoc />
    public virtual int TargetPositionX
    {
        get => _targetPositionX;
        set
        {
            if (_targetPositionX != value)
            {
                var oldx = _targetPositionX;
                _targetPositionX = value;
                OnTargetPositionChanged.Publish(new(this, oldx, _targetPositionY, _targetPositionX, _targetPositionY));
            }
        }
    }

    /// <inheritdoc />
    public virtual int TargetPositionY
    {
        get => _targetPositionY;
        set
        {
            if (_targetPositionY != value)
            {
                var oldY = _targetPositionY;
                _targetPositionY = value;
                OnTargetPositionChanged.Publish(new(this, _targetPositionX, oldY, _targetPositionX, _targetPositionY));
            }
        }
    }

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkSizeChangedEvent> OnSizeChanged { get; init; } = new TypedEvent<LinkSizeChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkTargetPortChangedEvent> OnTargetPortChanged { get; init; } =
        new TypedEvent<LinkTargetPortChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkSourcePortChangedEvent> OnSourcePortChanged { get; init; } =
        new TypedEvent<LinkSourcePortChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkTargetPositionChangedEvent> OnTargetPositionChanged { get; init; } =
        new TypedEvent<LinkTargetPositionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkSelectionChangedEvent> OnSelectionChanged { get; init; } =
        new TypedEvent<LinkSelectionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkVisibilityChangedEvent> OnVisibilityChanged { get; init; } =
        new TypedEvent<LinkVisibilityChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public virtual bool IsConnected => TargetPort is not null;


    /// <inheritdoc />
    public virtual bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnSelectionChanged.Publish(new(this));
            }
        }
    }


    /// <inheritdoc />
    public virtual bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible != value)
            {
                _isVisible = value;
                OnVisibilityChanged.Publish(new(this));
            }
        }
    }

    /// <inheritdoc />
    public virtual int Width
    {
        get => _width;
        set
        {
            if (_width != value)
            {
                var oldWidth = _width;
                _width = value;
                OnSizeChanged.Publish(new(this, oldWidth, _height, _width, _height));
            }
        }
    }

    /// <inheritdoc />
    public virtual int Height
    {
        get => _height;
        set
        {
            if (_height != value)
            {
                var oldHeight = _height;
                _height = value;
                OnSizeChanged.Publish(new(this, _width, oldHeight, _width, _height));
            }
        }
    }

    /// Severs the connection between link and ports.
    public virtual void Dispose()
    {
        _sourcePort?.OutgoingLinks.Remove(this);
        _targetPort?.IncomingLinks.Remove(this);
    }
}