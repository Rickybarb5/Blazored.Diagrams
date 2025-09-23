using Blazored.Diagrams.Components.Models;
using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Nodes;

/// <summary>
///     Base class for all nodes.
/// </summary>
public partial class Node : INode
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
        _ports.OnItemAdded += HandlePortAdded;
        _ports.OnItemRemoved += HandlePortRemoved;
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
                var oldY = _positionY;
                _positionY = value;
                OnPositionChanged?.Invoke(this, _positionX, oldY, _positionX, _positionY);
            }
        }
    }

    /// <inheritdoc />
    public event Action<INode, int, int, int, int>? OnPositionChanged;

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
    public virtual ObservableList<IPort> Ports
    {
        get => _ports;
        init
        {
            _ports.Clear();
            foreach (var val in value) _ports.Add(val);
        }
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        _ports.OnItemAdded -= HandlePortAdded;
        _ports.OnItemRemoved -= HandlePortRemoved;
        _ports.ForEach(x => x.Dispose());
        _ports.Clear();
    }


    /// <inheritdoc />
    public event Action<INode, int, int, int, int>? OnSizeChanged;

    /// <inheritdoc />
    public event Action<INode>? OnSelectionChanged;

    /// <inheritdoc />
    public event Action<INode>? OnVisibilityChanged;

    /// <inheritdoc />
    public event Action<INode, IPort>? OnPortAdded;

    /// <inheritdoc />
    public event Action<INode, IPort>? OnPortRemoved;
}