using Blazored.Diagrams.Events;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Links;

public partial class Link
{
    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkSizeChangedEvent> OnSizeChanged { get; init; } = new TypedEvent<LinkSizeChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkTargetPortChangedEvent> OnTargetPortChanged { get; init; } =
        new TypedEvent<LinkTargetPortChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkSourcePortChangedEvent> OnSourcePortChanged { get; init; } =
        new TypedEvent<LinkSourcePortChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkTargetPositionChangedEvent> OnTargetPositionChanged { get; init; } =
        new TypedEvent<LinkTargetPositionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkSelectionChangedEvent> OnSelectionChanged { get; init; } =
        new TypedEvent<LinkSelectionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LinkVisibilityChangedEvent> OnVisibilityChanged { get; init; } =
        new TypedEvent<LinkVisibilityChangedEvent>();
}