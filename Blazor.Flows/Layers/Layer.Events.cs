using Blazor.Flows.Events;
using Newtonsoft.Json;

namespace Blazor.Flows.Layers;

public partial class Layer
{
    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LayerVisibilityChangedEvent> OnVisibilityChanged { get; init; } =
        new TypedEvent<LayerVisibilityChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<NodeAddedToLayerEvent> OnNodeAddedToLayer { get; init; } =
        new TypedEvent<NodeAddedToLayerEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<NodeRemovedFromLayerEvent> OnNodeRemovedFromLayer { get; init; } =
        new TypedEvent<NodeRemovedFromLayerEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<GroupAddedToLayerEvent> OnGroupAddedToLayer { get; init; } =
        new TypedEvent<GroupAddedToLayerEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<GroupRemovedFromLayerEvent> OnGroupRemovedFromLayer { get; init; } =
        new TypedEvent<GroupRemovedFromLayerEvent>();

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
    
    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LayerZIndexChanged> OnZIndexChanged { get; init; } = new TypedEvent<LayerZIndexChanged>();
}