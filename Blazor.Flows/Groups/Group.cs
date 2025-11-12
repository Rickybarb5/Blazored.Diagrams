using Blazor.Flows.Components.Models;
using Blazor.Flows.Events;
using Blazor.Flows.Extensions;
using Blazor.Flows.Helpers;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;
using Blazor.Flows.Services.Registry;
using Newtonsoft.Json;

namespace Blazor.Flows.Groups;

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
    private int _zIndex;

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

    /// <inheritdoc />
    public virtual void Dispose()
    {
        _ports.ForEach(x => x.Dispose());
        _ports.ClearInternal();
        _nodes.ForEach(x => x.Dispose());
        _nodes.ClearInternal();
        _groups.ForEach(x => x.Dispose());
        _groups.ClearInternal();
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
        get => _positionX;
        set
        {
            if (_positionX != value)
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
        get => _positionY;
        set
        {
            if (_positionY != value)
            {
                var oldY = _positionX;
                _positionY = value;
                OnPositionChanged.Publish(new(this, _positionX, oldY, _positionX, _positionY));
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
    public virtual int Padding
    {
        get => _padding;
        set
        {
            if (_padding != value)
            {
                var oldPadding = _padding;
                _padding = value;
                OnPaddingChanged.Publish(new(this, oldPadding, _padding));
            }
        }
    }


    /// <inheritdoc />
    public virtual ObservableList<IPort> Ports
    {
        get => _ports;
        set
        {
            _ports.ClearInternal();
            value.ForEach(p => _ports.AddInternal(p));
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<INode> Nodes
    {
        get => _nodes;
        set
        {
            _nodes.ClearInternal();
            value.ForEach(n => _nodes.AddInternal(n));
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<IGroup> Groups
    {
        get => _groups;
        set
        {
            _groups.ClearInternal();
            value.ForEach(g => _groups.AddInternal(g));
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
        OnPortAddedToGroup.Publish(new(this, obj.Item));
        OnPortAdded.Publish(new(obj.Item));
    }

    /// <summary>
    /// Adds a port to the port list.
    /// This method is useful if you want to initialize a node with default ports.
    /// </summary>
    /// <param name="port">The port that will be added.</param>
    protected void AddPortInternal(IPort port)
    {
        Ports.AddInternal(port);
    }

    /// <summary>
    /// Adds a node to the node list.
    /// This method is useful if you want to initialize a node with default ports.
    /// </summary>
    /// <param name="node">Node to be added</param>
    protected void AddNodeInternal(INode node)
    {
        Nodes.AddInternal(node);
    }

    /// <summary>
    /// Adds a group to the group list.
    /// This method is useful if you want to initialize a node with default ports.
    /// </summary>
    /// <param name="group"></param>
    protected void AddGroupInternal(IGroup group)
    {
        Groups.AddInternal(group);
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