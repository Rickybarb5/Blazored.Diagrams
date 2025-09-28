using System.Text.Json.Serialization;
using Blazored.Diagrams.Components.Models;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Ports;

/// <summary>
///     Base class for a port.
/// </summary>
public partial class Port : IPort, IHasComponent<DefaultPortComponent>
{
    private readonly ObservableList<ILink> _incomingLinks = [];
    private readonly ObservableList<ILink> _outgoingLinks = [];
    private PortJustification _justification = PortJustification.Center;
    private int _height;
    private bool _isVisible = true;
    private IPortContainer _parent;
    private PortAlignment _alignment = PortAlignment.Left;
    private int _width;

    /// <summary>
    ///     Instantiates a new <see cref="Port" />
    /// </summary>
    public Port()
    {
        Anchor = new PortAnchor
        {
            Port = this,
        };
        
        _incomingLinks.OnItemAdded += HandleIncomingLinkAdded;
        _incomingLinks.OnItemRemoved += HandleIncomingLinkRemoved;
        _outgoingLinks.OnItemAdded += HandleOutgoingLinkAdded;
        _outgoingLinks.OnItemRemoved += HandleOutgoingLinkRemoved;
    }

    /// <inheritdoc />
    public virtual Guid Id { get; init; } = Guid.NewGuid();

    public IPortAnchor Anchor { get; set; }

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


    /// <inheritdoc />
    public virtual int PositionX
    {
        get => Anchor.PositionX + Anchor.OffsetX - Width/2 ;
        set
        {
            if (Anchor.PositionX != value)
            {
                var oldX = Anchor.PositionX;
                Anchor.PositionX = value;
                OnPositionChanged?.Invoke(this, oldX, Anchor.PositionY, Anchor.PositionX, Anchor.PositionY);
            }
        }
    }

    /// <inheritdoc />
    public virtual int PositionY
    {
        get => Anchor.PositionY + Anchor.OffsetY - Height/2;
        set
        {
            if (Anchor.PositionY != value)
            {
                var oldY = Anchor.PositionY;
                Anchor.PositionY = value;
                OnPositionChanged?.Invoke(this, Anchor.PositionX, oldY, Anchor.PositionX, Anchor.PositionY);
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
    public virtual PortJustification Justification
    {
        get => _justification;
        set
        {
            if (_justification != value)
            {
                var old = _justification;
                _justification = value;
                OnPortJustificationChanged?.Invoke(this, old, _justification);
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
                OnPortAlignmentChanged?.Invoke(this, old, _alignment);
            }
        }
    }


    /// <inheritdoc />
    public virtual ObservableList<ILink> OutgoingLinks
    {
        get => _outgoingLinks;
        init
        {
            _outgoingLinks.Clear();
            value.ForEach(val =>
            {
                _outgoingLinks.Add(val);
                val.SourcePort = this;
            });
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<ILink> IncomingLinks
    {
        get => _incomingLinks;
        init
        {
            _incomingLinks.Clear();
            value.ForEach(val =>
            {
                _incomingLinks.Add(val);
                val.TargetPort = this;
            });
        }
    }

    //TODO:Check if adding required feels good.
    /// <inheritdoc />
    [JsonIgnore]
    public virtual IPortContainer Parent
    {
        get => _parent;
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(Parent));
            if (_parent != value)
            {
                var oldParent = _parent;
                _parent = value;
                _parent.Ports.Add(this);
                OnPortParentChanged?.Invoke(this, oldParent, _parent);
            }
        }
    }
    
    /// <inheritdoc />
    public event Action<IPort>? OnVisibilityChanged;

    /// <inheritdoc />
    public event Action<IPort, ILink>? OnIncomingLinkAdded;

    /// <inheritdoc />
    public event Action<IPort, ILink>? OnIncomingLinkRemoved;

    /// <inheritdoc />
    public event Action<IPort, ILink>? OnOutgoingLinkAdded;

    /// <inheritdoc />
    public event Action<IPort, ILink>? OnOutgoingLinkRemoved;

    /// <inheritdoc />
    public event Action<IPort, PortJustification, PortJustification>? OnPortJustificationChanged;

    /// <inheritdoc />
    public event Action<IPort, IPortContainer, IPortContainer>? OnPortParentChanged;

    /// <inheritdoc />
    public event Action<IPort, PortAlignment, PortAlignment>? OnPortAlignmentChanged;

    /// <inheritdoc />
    public event Action<IPort, int, int, int, int>? OnPositionChanged;

    /// <inheritdoc />
    public event Action<IPort, int, int, int, int>? OnSizeChanged;

    /// <inheritdoc />
    public virtual void Dispose()
    {
        _incomingLinks.ForEach(x => x.Dispose());
        _outgoingLinks.ForEach(x => x.Dispose());
        _incomingLinks.Clear();
        _outgoingLinks.Clear();
        _parent.Ports.Remove(this);

        _incomingLinks.OnItemAdded -= HandleIncomingLinkAdded;
        _incomingLinks.OnItemRemoved -= HandleIncomingLinkRemoved;
        _outgoingLinks.OnItemAdded -= HandleOutgoingLinkAdded;
        _outgoingLinks.OnItemRemoved -= HandleOutgoingLinkRemoved;
    }
}