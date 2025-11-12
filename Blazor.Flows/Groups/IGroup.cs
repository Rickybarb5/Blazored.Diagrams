using Blazor.Flows.Events;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;
using Newtonsoft.Json;

namespace Blazor.Flows.Groups;

/// <summary>
///     Interface to implement basic group features.
/// </summary>
public interface IGroup :
    IId,
    IVisible,
    IDepth,
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
    /// Event triggered when the size changes.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<GroupSizeChangedEvent> OnSizeChanged { get; init; }

    /// <summary>
    /// Event triggered when the position changes
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<GroupPositionChangedEvent> OnPositionChanged { get; init; }

    /// <summary>
    /// Event triggered when the selection state changes.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<GroupSelectionChangedEvent> OnSelectionChanged { get; init; }

    /// <summary>
    /// EventTriggered when the visibility state changes
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<GroupVisibilityChangedEvent> OnVisibilityChanged { get; init; }

    /// <summary>
    ///     Event triggered when a port is added.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<PortAddedToGroupEvent> OnPortAddedToGroup { get; init; }

    /// <summary>
    ///     Event triggered when a port is removed.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<PortRemovedFromGroupEvent> OnPortRemovedFromGroup { get; init; }

    /// <summary>
    ///     Event triggered when a node is added.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<NodeAddedToGroupEvent> OnNodeAddedToGroup { get; init; }

    /// <summary>
    ///     Event triggered when a node is removed.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<NodeRemovedFromGroupEvent> OnNodeRemovedFromGroup { get; init; }

    /// <summary>
    ///     Event triggered when a nested group is added.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<GroupAddedToGroupEvent> OnGroupAddedToGroup { get; init; }

    /// <summary>
    ///     Event triggered when a nested group is removed.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<GroupRemovedFromGroupEvent> OnGroupRemovedFromGroup { get; init; }

    /// <summary>
    /// Event triggered when the padding changes.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<GroupPaddingChangedEvent> OnPaddingChanged { get; init; }


    /// <summary>
    ///     Event triggered when a port is added.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<PortAddedEvent> OnPortAdded { get; init; }

    /// <summary>
    ///     Event triggered when a port is removed.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<PortRemovedEvent> OnPortRemoved { get; init; }


    /// <summary>
    ///     Event triggered when a node is added.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<NodeAddedEvent> OnNodeAdded { get; init; }

    /// <summary>
    ///     Event triggered when a node is removed.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<NodeRemovedEvent> OnNodeRemoved { get; init; }


    /// <summary>
    ///     Event triggered when a group is added.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<GroupAddedEvent> OnGroupAdded { get; init; }

    /// <summary>
    ///     Event triggered when a group is removed.
    /// </summary>
    [JsonIgnore]
    public ITypedEvent<GroupRemovedEvent> OnGroupRemoved { get; init; }
        
    /// <summary>
    /// Event triggered when the <see cref="IGroup.ZIndex"/> changes.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<GroupZIndexChangedEvent> OnZIndexChanged { get; init; }

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
    /// Unselects all models in a group
    /// </summary>
    void UnselectAll();

    /// <summary>
    /// Selects all models in a group.
    /// </summary>
    void SelectAll();
}