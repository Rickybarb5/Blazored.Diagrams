using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Events;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Layers;

/// <summary>
///     Contains all methods relevant for layers.
/// </summary>
public interface ILayer : IId,
    IVisible,
    INodeContainer,
    IGroupContainer,
    IDisposable
{
    /// <summary>
    ///     Ports that are container within the layer.
    /// </summary>
    [JsonIgnore]
    IReadOnlyList<IPort> AllPorts { get; }

    /// <summary>
    ///     All top-level groups and nested groups.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<IGroup> AllGroups { get; }

    /// <summary>
    ///     All top-level nodes and nested nodes.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<INode> AllNodes { get; }

    /// <summary>
    ///     All link contained within this layer.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<ILink> AllLinks { get; }

    /// <summary>
    /// Event triggered when the visibility state changes.
    /// </summary>
    public ITypedEvent<LayerVisibilityChangedEvent> OnVisibilityChanged { get; init; }

    /// <summary>
    ///     Event triggered when a node is added.
    /// </summary>
    public ITypedEvent<NodeAddedToLayerEvent> OnNodeAddedToLayer { get; init; }

    /// <summary>
    ///     Event triggered when a node is removed.
    /// </summary>
    public ITypedEvent<NodeRemovedFromLayerEvent> OnNodeRemovedFromLayer { get; init; }

    /// <summary>
    ///     Event triggered when a nested group is added.
    /// </summary>
    public ITypedEvent<GroupAddedToLayerEvent> OnGroupAddedToLayer { get; init; }

    /// <summary>
    ///     Event triggered when a nested group is removed.
    /// </summary>
    public ITypedEvent<GroupRemovedFromLayerEvent> OnGroupRemovedFromLayer { get; init; }
        
    /// <summary>
    ///     Event triggered when a node is added.
    /// </summary>
    public ITypedEvent<NodeAddedEvent> OnNodeAdded { get; init; } 
    
    /// <summary>
    ///     Event triggered when a node is removed.
    /// </summary>
    public ITypedEvent<NodeRemovedEvent> OnNodeRemoved { get; init; } 
    
        
    /// <summary>
    ///     Event triggered when a group is added.
    /// </summary>
    public ITypedEvent<GroupAddedEvent> OnGroupAdded { get; init; } 
    
    /// <summary>
    ///     Event triggered when a group is removed.
    /// </summary>
    public ITypedEvent<GroupRemovedEvent> OnGroupRemoved { get; init; } 
    
    

    /// <summary>
    /// Unselects all models in a layer
    /// </summary>
    void UnselectAll();

    /// <summary>
    /// Selects ll models in a layer
    /// </summary>
    void SelectAll();
}