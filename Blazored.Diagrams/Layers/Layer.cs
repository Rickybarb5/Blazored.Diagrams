using System.Text.Json.Serialization;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Layers;

/// <summary>
///     Base implementation of a layer.
/// </summary>
public partial class Layer : ILayer
{
    private readonly ObservableList<IGroup> _groups = [];
    private readonly ObservableList<INode> _nodes = [];
    private bool _isVisible = true;
    private bool _currentLayer;


    public Layer()
    {
        _nodes.OnItemAdded += HandleNodeAdded;
        _nodes.OnItemRemoved += HandleNodeRemoved;

        _groups.OnItemAdded += HandleGroupAdded;
        _groups.OnItemRemoved += HandleGroupRemoved;
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

    /// <inheritdoc />
    public virtual Guid Id { get; init; } = Guid.NewGuid();

    /// <inheritdoc />
    public virtual ObservableList<INode> Nodes
    {
        get => _nodes;
        init
        {
            _nodes.Clear();
            value.ForEach(val=>_nodes.Add(val));
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<IGroup> Groups
    {
        get => _groups;
        init
        {
            _groups.Clear();
            
            value.ForEach(val=>_groups.Add(val));
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
    [JsonIgnore]
    public virtual IReadOnlyList<IGroup> AllGroups =>
        Groups
            .Concat(Groups.SelectMany(g => g.AllGroups))
            .DistinctBy(x => x.Id)
            .ToList()
            .AsReadOnly();

    /// <inheritdoc />
    [JsonIgnore]
    public virtual IReadOnlyList<INode> AllNodes =>
        Nodes
            .Concat(Groups.SelectMany(g => g.AllNodes))
            .ToList()
            .AsReadOnly();

    /// <inheritdoc />
    [JsonIgnore]
    public virtual IReadOnlyList<ILink> AllLinks => AllPorts
        .SelectMany(x => x.OutgoingLinks)
        .Concat(AllPorts.SelectMany(x => x.IncomingLinks))
        .DistinctBy(x => x.Id)
        .ToList()
        .AsReadOnly();


    /// <inheritdoc />
    public bool IsCurrentLayer
    {
        get => _currentLayer;
        set
        {
            if (value != _currentLayer)
            {
                _currentLayer = value;
                OnLayerUsageChanged?.Invoke(this);
            }
        }
    }

    /// <inheritdoc />
    [JsonIgnore]
    public virtual IReadOnlyList<IPort> AllPorts =>
        Nodes.SelectMany(x => x.Ports)
            .Concat(Groups.SelectMany(x => x.AllPorts))
            .DistinctBy(x => x.Id)
            .ToList()
            .AsReadOnly();


    /// <inheritdoc />
    public virtual void UnselectAll()
    {
        Nodes.ForEach(x => x.IsSelected = false);
        Groups.ForEach(x =>
        {
            x.IsSelected = false;
            x.UnselectAll();
        });
        AllLinks.ForEach(x => x.IsSelected = false);
    }

    /// <inheritdoc />
    public virtual void SelectAll()
    {
        Nodes.ForEach(x => x.IsSelected = true);
        Groups.ForEach(x =>
        {
            x.IsSelected = true;
            x.SelectAll();
        });
        AllLinks.ForEach(x => x.IsSelected = true);
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        _nodes.ForEach(x => x.Dispose());
        _nodes.Clear();
        _groups.ForEach(x => x.Dispose());
        _groups.Clear();
        _nodes.OnItemAdded -= HandleNodeAdded;
        _nodes.OnItemRemoved -= HandleNodeRemoved;

        _groups.OnItemAdded -= HandleGroupAdded;
        _groups.OnItemRemoved -= HandleGroupRemoved;
    }

    /// <inheritdoc />
    public event Action<ILayer>? OnLayerUsageChanged;

    /// <inheritdoc />
    public event Action<ILayer>? OnVisibilityChanged;

    /// <inheritdoc />
    public event Action<ILayer, INode>? OnNodeAdded;

    /// <inheritdoc />
    public event Action<ILayer, INode>? OnNodeRemoved;

    /// <inheritdoc />
    public event Action<ILayer, IGroup>? OnGroupAdded;

    /// <inheritdoc />
    public event Action<ILayer, IGroup>? OnGroupRemoved;
}