using System.Text.Json.Serialization;
using Blazored.Diagrams.Components.Models;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Links;

/// <summary>
/// Default link implementation.
/// </summary>
public partial class Link : ILink, IHasComponent<DefaultLinkComponent>
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
    public virtual Guid Id { get; init; } = Guid.NewGuid();


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
                OnSourcePortChanged?.Invoke(this, oldPort, _sourcePort);
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
                OnTargetPortChanged?.Invoke(this, oldTargetPort, _targetPort);
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
                OnTargetPositionChanged?.Invoke(this, oldx, _targetPositionY, _targetPositionX, _targetPositionY);
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
                OnTargetPositionChanged?.Invoke(this, _targetPositionX, oldY, _targetPositionX, _targetPositionY);
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
                OnSelectionChanged?.Invoke(this);
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
                OnVisibilityChanged?.Invoke(this);
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
                OnSizeChanged?.Invoke(this, oldWidth, _height, _width, _height);
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
                OnSizeChanged?.Invoke(this, _width, oldHeight, _width, _height);
            }
        }
    }

    /// Severs the connection between link and ports.
    public virtual void Dispose()
    {
        _sourcePort?.OutgoingLinks.Remove(this);
        _targetPort?.IncomingLinks.Remove(this);
    }

    /// <inheritdoc />
    public event Action<ILink, int, int, int, int>? OnSizeChanged;

    /// <inheritdoc />
    public event Action<ILink, IPort?, IPort?>? OnTargetPortChanged;

    /// <inheritdoc />
    public event Action<ILink, IPort, IPort>? OnSourcePortChanged;

    /// <inheritdoc />
    public event Action<ILink, int, int, int, int>? OnTargetPositionChanged;

    /// <inheritdoc />
    public event Action<ILink>? OnSelectionChanged;

    /// <inheritdoc />
    public event Action<ILink>? OnVisibilityChanged;
}