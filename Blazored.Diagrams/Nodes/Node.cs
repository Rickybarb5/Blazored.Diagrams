using Blazored.Diagrams.Components.Models;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Nodes;

/// <summary>
///     Base class for all nodes.
/// </summary>
public partial class Node : INode, IHasComponent<DefaultNodeComponent>
{
    private readonly ObservableList<IPort> _ports = [];
    private int _height;
    private bool _isSelected;
    private bool _isVisible = true;
    private int _positionX;
    private int _positionY;
    private int _width;

    /// <summary>
    /// Instantiates a new <see cref="Node"/>
    /// </summary>
    public Node()
    {
        _ports.OnItemAdded.Subscribe(HandlePortAdded);
        _ports.OnItemRemoved.Subscribe(HandlePortRemoved);
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
                OnSizeChanged.Publish(new NodeSizeChangedEvent(this, oldWidth, _height, _width, _height));
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
                OnSizeChanged.Publish(new NodeSizeChangedEvent(this, _width, oldHeight, _width, _height));
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
                OnPositionChanged.Publish(new NodePositionChangedEvent(this, oldX, _positionY, _positionX, _positionY));
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
                var oldY = _positionY;
                _positionY = value;
                OnPositionChanged.Publish(new NodePositionChangedEvent(this, _positionX, oldY, _positionX, _positionY));
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
                OnSelectionChanged.Publish(new NodeSelectionChangedEvent(this));
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
                OnVisibilityChanged.Publish(new NodeVisibilityChangedEvent(this));
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
            value.ForEach(val => _ports.AddInternal(val));
        }
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        _ports.ForEach(x => x.Dispose());
        _ports.ClearInternal();
        _ports.OnItemAdded.Unsubscribe(HandlePortAdded);
        _ports.OnItemRemoved.Unsubscribe(HandlePortRemoved);
    }


    private void HandlePortRemoved(ItemRemovedEvent<IPort> obj)
    {
        obj.Item.Dispose();
        OnPortRemovedFromNode.Publish(new(this, obj.Item));
        OnPortRemoved.Publish(new PortRemovedEvent(obj.Item));
    }

    private void HandlePortAdded(ItemAddedEvent<IPort> obj)
    {
        obj.Item.Parent = this;
        OnPortAddedToNode.Publish(new(this, obj.Item));
        OnPortAdded.Publish(new PortAddedEvent(obj.Item));
    }

    /// <summary>
    /// Adds a port to the port list.
    /// This method is useful if you want to initialize a node with default ports.
    /// </summary>
    /// <param name="port"></param>
    protected void AddPortInternal(IPort port)
    {
        Ports.AddInternal(port);
    }
}