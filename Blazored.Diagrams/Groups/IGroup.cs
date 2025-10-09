using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Events;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Groups;

/// <summary>
///     Interface to implement basic group features.
/// </summary>
public interface IGroup :
    IId,
    IVisible,
    INodeContainer,
    IGroupContainer,
    IPortContainer,
    ISelectable,
    IPadding,
    IDisposable
{
    /// <summary>
    ///     Gets all nodes and nested Nodes.
    /// </summary>
    [JsonIgnore]
    IReadOnlyList<INode> AllNodes { get; }

    /// <summary>
    ///     Gets all groups and nested groups.
    /// </summary>
    
    [JsonIgnore]
    IReadOnlyList<IGroup> AllGroups { get; }

    /// <summary>
    ///     Gets all ports and nested ports.
    /// </summary>
    
    [JsonIgnore]
    IReadOnlyList<IPort> AllPorts { get; }

    /// <summary>
    ///     Sets the X and Y coordinates on the screen.
    ///     Does not trigger the event.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void SetPositionInternal(int x, int y);

    /// <summary>
    ///     Sets the width and height.
    ///     Does not trigger the event.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    void SetSizeInternal(int width, int height);


    /// <summary>
    /// Event triggered when the size changes.
    /// </summary>
    public ITypedEvent<GroupSizeChangedEvent> OnSizeChanged { get; init; }

    /// <summary>
    /// Event triggered when the position changes
    /// </summary>
    public ITypedEvent<GroupPositionChangedEvent> OnPositionChanged { get; init; }

    /// <summary>
    /// Event triggered when the selection state changes.
    /// </summary>
    public ITypedEvent<GroupSelectionChangedEvent> OnSelectionChanged { get; init; }

    /// <summary>
    /// EventTriggered when the visibility state changes
    /// </summary>
    public ITypedEvent<GroupVisibilityChangedEvent> OnVisibilityChanged { get; init; }

    /// <summary>
    ///     Event triggered when a port is added.
    /// </summary>
    public ITypedEvent<PortAddedToGroupEvent> OnPortAddedToGroup { get; init; } 

    /// <summary>
    ///     Event triggered when a port is removed.
    /// </summary>
    public ITypedEvent<PortRemovedFromGroupEvent> OnPortRemovedFromGroup { get; init; }
    /// <summary>
    ///     Event triggered when a node is added.
    /// </summary>
    public ITypedEvent<NodeAddedToGroupEvent> OnNodeAddedToGroup { get; init; }

    /// <summary>
    ///     Event triggered when a node is removed.
    /// </summary>
    public ITypedEvent<NodeRemovedFromGroupEvent> OnNodeRemovedFromGroup { get; init; }

    /// <summary>
    ///     Event triggered when a nested group is added.
    /// </summary>
    public ITypedEvent<GroupAddedToGroupEvent> OnGroupAddedTogroup { get; init; }

    /// <summary>
    ///     Event triggered when a nested group is removed.
    /// </summary>
    public ITypedEvent<GroupRemovedFromGroupEvent> OnGroupRemovedFromGroup { get; init; }

    /// <summary>
    /// Event triggered when the padding changes.
    /// </summary>
    public ITypedEvent<GroupPaddingChangedEvent> OnPaddingChanged { get; init; }
    
    
    /// <summary>
    ///     Event triggered when a port is added.
    /// </summary>
    public ITypedEvent<PortAddedEvent> OnPortAdded { get; init; } 
    
    /// <summary>
    ///     Event triggered when a port is removed.
    /// </summary>
    public ITypedEvent<PortRemovedEvent> OnPortRemoved { get; init; } 
    
    
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
    /// Unselects all models in a group
    /// </summary>
    void UnselectAll();

    /// <summary>
    /// Selects all models in a group.
    /// </summary>
    void SelectAll();
}