using Blazored.Diagrams.Components.Models;
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
    private IPort _sourcePort = null!;
    private IPort? _targetPort;
    private int _targetPositionX;
    private int _targetPositionY;
    private int _width;
    private int _height;

    /// <inheritdoc />
    public virtual string Id { get; init; } = Guid.NewGuid().ToString();

    /// <summary>
    ///     Port where the link originates from.
    /// </summary>
    [JsonIgnore]
    public virtual IPort SourcePort
    {
        get => _sourcePort;
        internal set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (_sourcePort != value)
            {
                var oldPort = _sourcePort;
                _sourcePort = value;
                _sourcePort.OutgoingLinks.AddInternal(this);
                OnSourcePortChanged.Publish(new(this, oldPort, _sourcePort));
            }
        }
    }

    [JsonIgnore]
    IPort ILink.SourcePort
    {
        get => SourcePort;
        set => SourcePort = value;
    }

    /// <summary>
    ///     End port that the link connects to, if it exists.
    /// </summary>
    [JsonIgnore]
    public virtual IPort? TargetPort
    {
        get => _targetPort;
        internal set 
        {
            if (_targetPort != value)
            {
                var oldTargetPort = _targetPort;
                _targetPort = value;
                _targetPort?.IncomingLinks.AddInternal(this);
                OnTargetPortChanged.Publish(new(this, oldTargetPort, _targetPort));
            }
        }
    }

    [JsonIgnore]
    IPort? ILink.TargetPort
    {
        get => TargetPort;
        set => TargetPort = value; 
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

    /// <inheritdoc />
    public virtual void Dispose()
    {
        _sourcePort?.OutgoingLinks.RemoveInternal(this);
        _targetPort?.IncomingLinks.RemoveInternal(this);
    }
}