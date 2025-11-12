using Blazor.Flows.Events;
using Newtonsoft.Json;

namespace Blazor.Flows.Nodes;

public partial class Node
{
    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<NodePositionChangedEvent> OnPositionChanged { get; init; } =
        new TypedEvent<NodePositionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<NodeSizeChangedEvent> OnSizeChanged { get; init; } = new TypedEvent<NodeSizeChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<NodeSelectionChangedEvent> OnSelectionChanged { get; init; } =
        new TypedEvent<NodeSelectionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<NodeVisibilityChangedEvent> OnVisibilityChanged { get; init; } =
        new TypedEvent<NodeVisibilityChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortAddedEvent> OnPortAdded { get; init; } = new TypedEvent<PortAddedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortRemovedEvent> OnPortRemoved { get; init; } = new TypedEvent<PortRemovedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortAddedToNodeEvent> OnPortAddedToNode { get; init; } = new TypedEvent<PortAddedToNodeEvent>();
    
    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortRemovedFromNodeEvent> OnPortRemovedFromNode { get; init; } =
        new TypedEvent<PortRemovedFromNodeEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<NodeZIndexChanged> OnZIndexChanged { get; init; } = new TypedEvent<NodeZIndexChanged>();
}