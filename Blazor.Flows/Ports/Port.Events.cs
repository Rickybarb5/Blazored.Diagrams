using Blazor.Flows.Events;
using Newtonsoft.Json;

namespace Blazor.Flows.Ports;

public partial class Port
{
    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortJustificationChangedEvent> OnPortJustificationChanged { get; init; } =
        new TypedEvent<PortJustificationChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortAlignmentChangedEvent> OnPortAlignmentChanged { get; init; } =
        new TypedEvent<PortAlignmentChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortPositionChangedEvent> OnPositionChanged { get; init; } =
        new TypedEvent<PortPositionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortSizeChangedEvent> OnSizeChanged { get; init; } = new TypedEvent<PortSizeChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortParentChangedEvent> OnPortParentChanged { get; init; } =
        new TypedEvent<PortParentChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortVisibilityChangedEvent> OnVisibilityChanged { get; init; } =
        new TypedEvent<PortVisibilityChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<IncomingLinkAddedEvent> OnIncomingLinkAdded { get; init; } =
        new TypedEvent<IncomingLinkAddedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<IncomingLinkRemovedEvent> OnIncomingLinkRemoved { get; init; } =
        new TypedEvent<IncomingLinkRemovedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<OutgoingLinkAddedEvent> OnOutgoingLinkAdded { get; init; } =
        new TypedEvent<OutgoingLinkAddedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<OutgoingLinkRemovedEvent> OnOutgoingLinkRemoved { get; init; } =
        new TypedEvent<OutgoingLinkRemovedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkRemovedEvent> OnLinkRemoved { get; init; } = new TypedEvent<LinkRemovedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkAddedEvent> OnLinkAdded { get; init; } = new TypedEvent<LinkAddedEvent>();
    
    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortSelectionChangedEvent> OnSelectionChanged { get; init; } =
        new TypedEvent<PortSelectionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<PortZIndexChanged> OnZIndexChanged { get; init; } = new TypedEvent<PortZIndexChanged>();
}