using Blazor.Flows.Components.Models;
using Blazor.Flows.Extensions;
using Blazor.Flows.Helpers;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Links;
using Blazor.Flows.Services.Registry;
using Newtonsoft.Json;

namespace Blazor.Flows.Ports;

/// <summary>
///     Base class for a port.
/// </summary>
public partial class Port : IPort, IHasComponent<DefaultPortComponent>
{
    private readonly ObservableList<ILink> _incomingLinks = [];
    private readonly ObservableList<ILink> _outgoingLinks = [];
    private PortAlignment _alignment = PortAlignment.Left;
    private int _height;
    private bool _isVisible = true;
    private PortJustification _justification = PortJustification.Center;
    private int _offsetX;
    private int _offsetY;
    private IPortContainer _parent = null!;
    private int _positionX;
    private int _positionY;
    private bool _selected;
    private int _width;
    private int _zIndex;

    /// <summary>
    ///     Instantiates a new <see cref="Port" />
    /// </summary>
    public Port()
    {
        _incomingLinks.OnItemAdded.Subscribe(HandleIncomingLinkAdded);
        _incomingLinks.OnItemRemoved.Subscribe(HandleIncomingLinkRemoved);
        _outgoingLinks.OnItemAdded.Subscribe(HandleOutgoingLinkAdded);
        _outgoingLinks.OnItemRemoved.Subscribe(HandleOutgoingLinkRemoved);
    }

    /// <inheritdoc />
    public virtual string Id { get; init; } = Guid.NewGuid().ToString();

    /// <inheritdoc />
    [JsonIgnore]
    public virtual bool HasLinks => HasOutGoingLinks || HasIncomingLinks;

    /// <inheritdoc />
    [JsonIgnore]
    public virtual bool HasIncomingLinks => _incomingLinks.Count != 0;

    /// <inheritdoc />
    [JsonIgnore]
    public virtual bool HasOutGoingLinks => _outgoingLinks.Count != 0;

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
    public virtual int PositionX
    {
        get => _positionX + OffSetX;
        set
        {
            if (value != _positionX)
            {
                var oldX = _positionX;
                _positionX = value;
                OnPositionChanged.Publish(new(this, oldX, _positionY, _positionX, _positionY));
            }
        }
    }

    /// <inheritdoc />
    public virtual int PositionY
    {
        get => _positionY + OffsetY;
        set
        {
            if (value != _positionY)
            {
                var oldY = _positionY;
                _positionY = value;
                OnPositionChanged.Publish(new(this, _positionX, oldY, _positionX, _positionY));
            }
        }
    }
    
    /// <inheritdoc />
    public virtual int OffSetX
    {
        get => _offsetX;
        set
        {
            if (value != _offsetX)
            {
                var oldX = _offsetX;
                _offsetX = value;
                OnPositionChanged.Publish(new(this, oldX, PositionY, PositionX, PositionY));
            }
        }
    }

    /// <inheritdoc />
    public virtual int OffsetY
    {
        get => _offsetY;
        set
        {
            if (value != _offsetY)
            {
                var oldY = _offsetY;
                _offsetY = value;
                OnPositionChanged.Publish(new(this, PositionX, oldY, PositionX, PositionY));
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
    public virtual PortJustification Justification
    {
        get => _justification;
        set
        {
            if (_justification != value)
            {
                var old = _justification;
                _justification = value;
                OnPortJustificationChanged.Publish(new(this, old, _justification));
            }
        }
    }

    /// <inheritdoc />
    public virtual PortAlignment Alignment
    {
        get => _alignment;
        set
        {
            if (_alignment != value)
            {
                var old = _alignment;
                _alignment = value;
                OnPortAlignmentChanged.Publish(new(this, old, _alignment));
            }
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<ILink> OutgoingLinks
    {
        get => _outgoingLinks;
        set
        {
            _outgoingLinks.ClearInternal();
            value.ForEach(val =>
            {
                _outgoingLinks.AddInternal(val);
                val.SourcePort = this;
            });
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<ILink> IncomingLinks
    {
        get => _incomingLinks;
        set
        {
            _incomingLinks.ClearInternal();
            value.ForEach(val =>
            {
                _incomingLinks.AddInternal(val);
                val.TargetPort = this;
            });
        }
    }

    /// <summary>
    ///     Parent of the port.
    /// </summary>
    [JsonIgnore]
    public virtual IPortContainer Parent
    {
        get => _parent;
        internal set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(Parent));
            if (_parent != value)
            {
                var oldParent = _parent;
                _parent = value;
                _parent.Ports.AddInternal(this);
                OnPortParentChanged.Publish(new(this, oldParent, _parent));
            }
        }
    }

    /// <inheritdoc />
    [JsonIgnore]
    IPortContainer IPort.Parent
    {
        get => Parent;
        set => Parent = value;
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        _incomingLinks.ForEach(x => x.Dispose());
        _outgoingLinks.ForEach(x => x.Dispose());
        _incomingLinks.ClearInternal();
        _outgoingLinks.ClearInternal();
        _parent.Ports.RemoveInternal(this);

        _incomingLinks.OnItemAdded.Unsubscribe(HandleIncomingLinkAdded);
        _incomingLinks.OnItemRemoved.Unsubscribe(HandleIncomingLinkRemoved);
        _outgoingLinks.OnItemAdded.Unsubscribe(HandleOutgoingLinkAdded);
        _outgoingLinks.OnItemRemoved.Unsubscribe(HandleOutgoingLinkRemoved);
    }

    /// <inheritdoc />
    public bool IsSelected
    {
        get => _selected;
        set
        {
            if (value != _selected)
            {
                _selected = value;
                OnSelectionChanged.Publish(new(this));
            }
        }
    }
    
    
    /// <inheritdoc />
    public int ZIndex
    {
        get => _zIndex;
        set
        {
            if (_zIndex != value)
            {
                _zIndex = value;
                OnZIndexChanged.Publish(new (this));
            }
        } 
    }
}