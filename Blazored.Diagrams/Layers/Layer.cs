
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Layers;

/// <summary>
///     Base implementation of a layer.
/// </summary>
public partial class Layer : ILayer
{
    private readonly ObservableList<IGroup> _groups = [];
    private readonly ObservableList<INode> _nodes = [];
    private bool _isVisible = true;


    /// <summary>
    /// Instantiates a new Layer.
    /// </summary>
    public Layer()
    {
        _nodes.OnItemAdded.Subscribe(HandleNodeAdded); 
        _nodes.OnItemRemoved.Subscribe(HandleNodeRemoved);

        _groups.OnItemAdded.Subscribe(HandleGroupAdded);
        _groups.OnItemRemoved.Subscribe(HandleGroupRemoved);
    }

    private void HandleGroupRemoved(ItemRemovedEvent<IGroup> obj)
    {
        OnGroupRemovedFromLayer.Publish(new(this, obj.Item));
        OnGroupRemoved.Publish(new(obj.Item));
    }

    private void HandleGroupAdded(ItemAddedEvent<IGroup> obj)
    {
        OnGroupAddedToLayer.Publish(new(this, obj.Item));
        OnGroupAdded.Publish(new(obj.Item));
    }

    private void HandleNodeRemoved(ItemRemovedEvent<INode> e)
    {
        OnNodeRemovedFromLayer.Publish(new(this, e.Item));
        OnNodeRemoved.Publish(new(e.Item));
    }

    private void HandleNodeAdded(ItemAddedEvent<INode> e)
    {
        OnNodeAddedToLayer.Publish(new(this, e.Item));
        OnNodeAdded.Publish(new(e.Item));
    }

    /// <inheritdoc />
    public virtual string Id { get; init; } = Guid.NewGuid().ToString();

    /// <inheritdoc />
    public virtual ObservableList<INode> Nodes
    {
        get => _nodes;
        set
        {
            _nodes.Clear();
            value.ForEach(val=>_nodes.Add(val));
        }
    }

    /// <inheritdoc />
    public virtual ObservableList<IGroup> Groups
    {
        get => _groups;
        set
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
                OnVisibilityChanged.Publish(new(this));
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
    public IReadOnlyList<ISelectable> SelectedModels => 
        AllNodes.Cast<ISelectable>()
        .Concat(AllGroups)
        .Concat(AllLinks)
        .Concat(AllPorts)
        .ToList()
        .AsReadOnly();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<LayerVisibilityChangedEvent> OnVisibilityChanged { get; init; } = new TypedEvent<LayerVisibilityChangedEvent>();
    
    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<NodeAddedToLayerEvent> OnNodeAddedToLayer { get; init; } = new TypedEvent<NodeAddedToLayerEvent>();
    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<NodeRemovedFromLayerEvent> OnNodeRemovedFromLayer { get; init; } = new TypedEvent<NodeRemovedFromLayerEvent>();
    
    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupAddedToLayerEvent> OnGroupAddedToLayer { get; init; } = new TypedEvent<GroupAddedToLayerEvent>();
    
    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupRemovedFromLayerEvent> OnGroupRemovedFromLayer { get; init; } = new TypedEvent<GroupRemovedFromLayerEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<NodeAddedEvent> OnNodeAdded { get; init; } =  new TypedEvent<NodeAddedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<NodeRemovedEvent> OnNodeRemoved { get; init; } =  new TypedEvent<NodeRemovedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupAddedEvent> OnGroupAdded { get; init; } = new TypedEvent<GroupAddedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
public ITypedEvent<GroupRemovedEvent> OnGroupRemoved { get; init; } = new TypedEvent<GroupRemovedEvent>();

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
        AllPorts.ForEach(x => x.IsSelected = false);
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
        AllPorts.ForEach(x => x.IsSelected = true);
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        _nodes.ForEach(x => x.Dispose());
        _nodes.Clear();
        _groups.ForEach(x => x.Dispose());
        _groups.Clear();
        _nodes.OnItemAdded.Unsubscribe(HandleNodeAdded); 
        _nodes.OnItemRemoved.Unsubscribe(HandleNodeRemoved);

        _groups.OnItemAdded.Unsubscribe(HandleGroupAdded);
        _groups.OnItemRemoved.Unsubscribe(HandleGroupRemoved);
    }
}