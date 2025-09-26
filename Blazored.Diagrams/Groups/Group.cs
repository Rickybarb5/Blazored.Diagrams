using System.Text.Json.Serialization;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Groups;

/// <summary>
///     Base implementation of a group.
/// </summary>
public partial class Group : IGroup
{
    private readonly ObservableList<IGroup> _groups = [];
    private readonly ObservableList<INode> _nodes = [];
    private readonly ObservableList<IPort> _ports = [];
    private int _height;
    private bool _isSelected;
    private bool _isVisible = true;
    private int _padding = 20;
    private int _positionX;
    private int _positionY;
    private int _width;

    /// <summary>
    ///     Initializes a new <see cref="Group"/>
    /// </summary>
    public Group()
    {
        _nodes.OnItemAdded += HandleNodeAdded;
        _nodes.OnItemRemoved += HandleNodeRemoved;
        _groups.OnItemAdded += HandleGroupAdded;
        _groups.OnItemRemoved += HandleGroupRemoved;
        _ports.OnItemAdded += HandlePortAdded;
        _ports.OnItemRemoved += HandlePortRemoved;
    }

    private void HandleGroupRemoved(IGroup obj)
    {
        OnGroupRemoved?.Invoke(this, obj);
    }

    private void HandleGroupAdded(IGroup obj)
    {
        OnGroupAdded?.Invoke(this, obj);
    }

    private void HandleNodeRemoved(INode obj)
    {
        OnNodeRemoved?.Invoke(this, obj);
    }

    private void HandleNodeAdded(INode obj)
    {
        OnNodeAdded?.Invoke(this, obj);
    }

    private void HandlePortRemoved(IPort obj)
    {
        obj.Dispose();
        OnPortRemoved?.Invoke(this, obj);
    }

    private void HandlePortAdded(IPort obj)
    {
        obj.Parent = this;
        OnPortAdded?.Invoke(this, obj);
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        _ports.ForEach(x => x.Dispose());
        _ports.Clear();
        _nodes.ForEach(x => x.Dispose());
        _nodes.Clear();
        _groups.ForEach(x => x.Dispose());
        _groups.Clear();
        _nodes.OnItemAdded -= HandleNodeAdded;
        _nodes.OnItemRemoved -= HandleNodeRemoved;

        _groups.OnItemAdded -= HandleGroupAdded;
        _groups.OnItemRemoved -= HandleGroupRemoved;

        _ports.OnItemAdded -= HandlePortAdded;
        _ports.OnItemRemoved -= HandlePortRemoved;
    }

    /// <inheritdoc />
    public virtual Guid Id { get; init; } = Guid.NewGuid();

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
        get => _positionX;
        set
        {
            if (_positionX != value)
            {
                var oldX = _positionX;
                _positionX = value;
                OnPositionChanged?.Invoke(this, oldX, _positionY, _positionX, _positionY);
            }
        }
    }

    /// <inheritdoc />
    public virtual int PositionY
    {
        get => _positionY;
        set
        {
            if (_positionY != value)
            {
                var oldY = _positionX;
                _positionY = value;
                OnPositionChanged?.Invoke(this, _positionX, oldY, _positionX, _positionY);
            }
        }
    }

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
    public virtual int Padding
    {
        get => _padding;
        set
        {
            if (_padding != value)
            {
                var oldPadding = _padding;
                _padding = value;
                OnPaddingChanged?.Invoke(this, oldPadding, _padding);
            }
        }
    }


    /// <inheritdoc />
    public virtual ObservableList<IPort> Ports
    {
        get => _ports;
        init
        {
            _ports.Clear();
            value.ForEach(p=>_ports.Add(p));
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<INode> Nodes
    {
        get => _nodes;
        init
        {
            _nodes.Clear();
            value.ForEach(n=>_nodes.Add(n));
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<IGroup> Groups
    {
        get => _groups;
        init
        {
            _groups.Clear();
            value.ForEach(g=>_groups.Add(g));
        }
    }


    /// <inheritdoc />
    [JsonIgnore]
    public virtual IReadOnlyList<INode> AllNodes =>
        Nodes
            .Concat(Groups.SelectMany(g => g.AllNodes))
            .ToList()
            .AsReadOnly();

    /// <inheritdoc />
    [JsonIgnore]
    public virtual IReadOnlyList<IGroup> AllGroups =>
        Groups
            .Concat(Groups.SelectMany(g => g.AllGroups))
            .ToList()
            .AsReadOnly();

    /// <inheritdoc />
    [JsonIgnore]
    public virtual IReadOnlyList<IPort> AllPorts =>
        Ports
            .Concat(AllGroups.SelectMany(g => g.AllPorts))
            .Concat(AllNodes.SelectMany(n => n.Ports))
            .ToList()
            .AsReadOnly();


    /// <inheritdoc />
    public event Action<IGroup, int, int>? OnPaddingChanged;

    /// <inheritdoc />
    public event Action<IGroup, int, int, int, int>? OnSizeChanged;

    /// <inheritdoc />
    public event Action<IGroup, int, int, int, int>? OnPositionChanged;

    /// <inheritdoc />
    public event Action<IGroup>? OnSelectionChanged;

    /// <inheritdoc />
    public event Action<IGroup>? OnVisibilityChanged;

    /// <inheritdoc />
    public event Action<IGroup, INode>? OnNodeAdded;

    /// <inheritdoc />
    public event Action<IGroup, INode>? OnNodeRemoved;

    /// <inheritdoc />
    public event Action<IGroup, IGroup>? OnGroupAdded;

    /// <inheritdoc />
    public event Action<IGroup, IGroup>? OnGroupRemoved;

    /// <inheritdoc />
    public event Action<IGroup, IPort>? OnPortAdded;

    /// <inheritdoc />
    public event Action<IGroup, IPort>? OnPortRemoved;
}