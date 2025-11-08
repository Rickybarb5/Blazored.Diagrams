using Blazored.Diagrams.Events;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Diagrams;

public partial class Diagram
{
    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<DiagramSizeChangedEvent> OnSizeChanged { get; init; } =
        new TypedEvent<DiagramSizeChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<DiagramZoomChangedEvent> OnZoomChanged { get; init; } =
        new TypedEvent<DiagramZoomChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<DiagramPanChangedEvent> OnPanChanged { get; init; } = new TypedEvent<DiagramPanChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LayerAddedEvent> OnLayerAdded { get; init; } = new TypedEvent<LayerAddedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<LayerRemovedEvent> OnLayerRemoved { get; init; } = new TypedEvent<LayerRemovedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<DiagramPositionChangedEvent> OnPositionChanged { get; init; } =
        new TypedEvent<DiagramPositionChangedEvent>();

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<CurrentLayerChangedEvent> OnCurrentLayerChanged { get; init; } =
        new TypedEvent<CurrentLayerChangedEvent>();
}