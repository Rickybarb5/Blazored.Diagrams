
using Blazored.Diagrams.Components.Models;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Registry;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Groups;

/// <summary>
///     Base implementation of a group.
/// </summary>
public partial class Group : IGroup, IHasComponent<DefaultGroupComponent>
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
        _nodes.OnItemAdded.Subscribe(HandleNodeAdded);
        _nodes.OnItemRemoved.Subscribe(HandleNodeRemoved);
        _groups.OnItemAdded.Subscribe(HandleGroupAdded);
        _groups.OnItemRemoved.Subscribe(HandleGroupRemoved);
        _ports.OnItemAdded.Subscribe(HandlePortAdded);
        _ports.OnItemRemoved.Subscribe(HandlePortRemoved);
    }

    private void HandleGroupRemoved(ItemRemovedEvent<IGroup> obj)
    {
        OnGroupRemovedFromGroup.Publish(new(this, obj.Item));
        OnGroupRemoved.Publish(new(obj.Item));
    }

    private void HandleGroupAdded(ItemAddedEvent<IGroup> obj)
    {
        OnGroupAddedToGroup.Publish(new(this, obj.Item));
        OnGroupAdded.Publish(new(obj.Item));
    }

    private void HandleNodeRemoved(ItemRemovedEvent<INode> obj)
    {
        OnNodeRemovedFromGroup.Publish(new(this, obj.Item));
        OnNodeRemoved.Publish(new(obj.Item));
    }

    private void HandleNodeAdded(ItemAddedEvent<INode> obj)
    {
        OnNodeAddedToGroup.Publish(new(this, obj.Item));
        OnNodeAdded.Publish(new(obj.Item));
    }

    private void HandlePortRemoved(ItemRemovedEvent<IPort> obj)
    {
        obj.Item.Dispose();
        OnPortRemovedFromGroup.Publish(new(this, obj.Item));
        OnPortRemoved.Publish(new(obj.Item));
    }

    private void HandlePortAdded(ItemAddedEvent<IPort> obj)
    {
        obj.Item.Parent = this;
        OnPortAddedToGroup?.Publish(new(this, obj.Item));
        OnPortAdded.Publish(new(obj.Item));
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
        _nodes.OnItemAdded.Unsubscribe(HandleNodeAdded);
        _nodes.OnItemRemoved.Unsubscribe(HandleNodeRemoved);

        _groups.OnItemAdded.Unsubscribe(HandleGroupAdded);
        _groups.OnItemRemoved.Unsubscribe(HandleGroupRemoved);

        _ports.OnItemAdded.Unsubscribe(HandlePortAdded);
        _ports.OnItemRemoved.Unsubscribe(HandlePortRemoved);
    }

    /// <inheritdoc />
    public virtual string Id { get; init; } = Guid.NewGuid().ToString();

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
                OnSizeChanged?.Publish(new(this, oldWidth, _height, _width, _height));
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
                OnSizeChanged?.Publish(new(this, _width, oldHeight, _width, _height));
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
                OnPositionChanged?.Publish(new(this, oldX, _positionY, _positionX, _positionY));
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
                OnPositionChanged?.Publish(new(this, _positionX, oldY, _positionX, _positionY));
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
                OnSelectionChanged?.Publish(new (this));
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
                OnVisibilityChanged?.Publish(new(this));
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
                OnPaddingChanged?.Publish(new(this, oldPadding, _padding));
            }
        }
    }


    /// <inheritdoc />
    public virtual ObservableList<IPort> Ports
    {
        get => _ports;
        set
        {
            _ports.Clear();
            value.ForEach(p=>_ports.Add(p));
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<INode> Nodes
    {
        get => _nodes;
        set
        {
            _nodes.Clear();
            value.ForEach(n=>_nodes.Add(n));
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<IGroup> Groups
    {
        get => _groups;
        set
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
    [JsonIgnore]
public ITypedEvent<GroupSizeChangedEvent> OnSizeChanged { get; init; } = new TypedEvent<GroupSizeChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupPositionChangedEvent> OnPositionChanged { get; init; } =
        new TypedEvent<GroupPositionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupSelectionChangedEvent> OnSelectionChanged { get; init; } =
        new TypedEvent<GroupSelectionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupVisibilityChangedEvent> OnVisibilityChanged { get; init; } =
        new TypedEvent<GroupVisibilityChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<PortAddedToGroupEvent> OnPortAddedToGroup { get; init; } = new TypedEvent<PortAddedToGroupEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<PortRemovedFromGroupEvent> OnPortRemovedFromGroup { get; init; } =
        new TypedEvent<PortRemovedFromGroupEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<NodeAddedToGroupEvent> OnNodeAddedToGroup { get; init; } = new TypedEvent<NodeAddedToGroupEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<NodeRemovedFromGroupEvent> OnNodeRemovedFromGroup { get; init; } =
        new TypedEvent<NodeRemovedFromGroupEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupAddedToGroupEvent> OnGroupAddedToGroup { get; init; } = new TypedEvent<GroupAddedToGroupEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupRemovedFromGroupEvent> OnGroupRemovedFromGroup { get; init; } =
        new TypedEvent<GroupRemovedFromGroupEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupPaddingChangedEvent> OnPaddingChanged { get; init; } =
        new TypedEvent<GroupPaddingChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<PortAddedEvent> OnPortAdded { get; init; } = new TypedEvent<PortAddedEvent>();
    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<PortRemovedEvent> OnPortRemoved { get; init; } = new TypedEvent<PortRemovedEvent>();
    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<NodeAddedEvent> OnNodeAdded { get; init; } =  new TypedEvent<NodeAddedEvent>();
    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<NodeRemovedEvent> OnNodeRemoved { get; init; } =   new TypedEvent<NodeRemovedEvent>();
    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupAddedEvent> OnGroupAdded { get; init; } =  new TypedEvent<GroupAddedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupRemovedEvent> OnGroupRemoved { get; init; } = new TypedEvent<GroupRemovedEvent>();
}