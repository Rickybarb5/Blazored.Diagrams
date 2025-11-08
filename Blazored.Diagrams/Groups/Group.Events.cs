using Blazored.Diagrams.Events;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Groups;

public partial class Group
{
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
    public ITypedEvent<PortAddedToGroupEvent> OnPortAddedToGroup { get; init; } =
        new TypedEvent<PortAddedToGroupEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortRemovedFromGroupEvent> OnPortRemovedFromGroup { get; init; } =
        new TypedEvent<PortRemovedFromGroupEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<NodeAddedToGroupEvent> OnNodeAddedToGroup { get; init; } =
        new TypedEvent<NodeAddedToGroupEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<NodeRemovedFromGroupEvent> OnNodeRemovedFromGroup { get; init; } =
        new TypedEvent<NodeRemovedFromGroupEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<GroupAddedToGroupEvent> OnGroupAddedToGroup { get; init; } =
        new TypedEvent<GroupAddedToGroupEvent>();

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
    public ITypedEvent<NodeAddedEvent> OnNodeAdded { get; init; } = new TypedEvent<NodeAddedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<NodeRemovedEvent> OnNodeRemoved { get; init; } = new TypedEvent<NodeRemovedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<GroupAddedEvent> OnGroupAdded { get; init; } = new TypedEvent<GroupAddedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<GroupRemovedEvent> OnGroupRemoved { get; init; } = new TypedEvent<GroupRemovedEvent>();
}