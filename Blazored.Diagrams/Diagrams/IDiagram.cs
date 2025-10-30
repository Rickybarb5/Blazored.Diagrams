using Blazored.Diagrams.Events;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Diagram;
using Blazored.Diagrams.Ports;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Diagrams;

/// <summary>
///     Represents a diagram.
/// </summary>
public interface IDiagram :
    IId,
    IPan,
    IZoomable,
    ILayerContainer,
    IDisposable
{
    /// <summary>
    ///     Current width of the diagram.
    /// </summary>
    int Width { get; set; }

    /// <summary>
    ///     Current height of the diagram.
    /// </summary>
    int Height { get; set; }

    /// <summary>
    ///     Gets all the nodes from all layers.
    /// </summary>
    [JsonIgnore]
    IReadOnlyList<INode> AllNodes { get; }

    /// <summary>
    ///     Gets the links from all layers.
    /// </summary>
    [JsonIgnore]
    IReadOnlyList<ILink> AllLinks { get; }

    /// <summary>
    ///     Gets the groups from all layers.
    /// </summary>
    [JsonIgnore]
    IReadOnlyList<IGroup> AllGroups { get; }

    /// <summary>
    /// Gets all ports in the diagram.
    /// </summary>
    [JsonIgnore]
    IReadOnlyList<IPort> AllPorts { get; }
    
    /// <summary>
    /// Gets the selected models in the diagram.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<ISelectable> SelectedModels { get; }

    /// <summary>
    /// Gets the current active layer.
    /// </summary>
    ILayer CurrentLayer { get; set; }

    /// <summary>
    /// Top left position of the diagram.
    /// </summary>
    int PositionX { get; set; }

    /// <summary>
    /// Top right position of the diagram.
    /// </summary>
    int PositionY { get; set; }

    /// <summary>
    /// Virtualization options.
    /// </summary>
    IDiagramOptions Options { get; init; }

    /// <summary>
    /// Sets the size of the diagram.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    internal void SetSize(int width, int height);

    /// <summary>
    /// Sets the position of the diagram
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    internal void SetPosition(int x, int y);

    /// <summary>
    ///     Event triggered when the diagram position is changed.
    /// </summary>
    ITypedEvent<DiagramSizeChangedEvent> OnSizeChanged { get; init; }

    /// <summary>
    /// Event triggered when the zoom value changes.
    /// </summary>
    ITypedEvent<DiagramZoomChangedEvent> OnZoomChanged { get; init; }

    /// <summary>
    /// Event triggered when the pan value changes.
    /// </summary>
    ITypedEvent<DiagramPanChangedEvent> OnPanChanged { get; init; }

    /// <summary>
    /// Event triggered when a layer is added.
    /// </summary>
    ITypedEvent<LayerAddedEvent> OnLayerAdded { get; init; }

    /// <summary>
    /// Event triggered when a layer is removed.
    /// </summary>
    ITypedEvent<LayerRemovedEvent> OnLayerRemoved { get; init; }

    /// <summary>
    /// Event triggered when the diagram position changes.
    /// </summary>
    ITypedEvent<DiagramPositionChangedEvent> OnPositionChanged { get; init; }

    /// <summary>
    /// Event triggered when the layer starts/stops being used.
    /// </summary>
    ITypedEvent<CurrentLayerChangedEvent> OnCurrentLayerChanged { get; init; }

    /// <summary>
    /// Unselects all models in the diagram.
    /// </summary>
    void UnselectAll();

    /// <summary>
    /// Selects all models in the diagram.
    /// </summary>
    void SelectAll();

    /// <summary>
    /// Sets the diagram zoom.
    /// </summary>
    /// <param name="zoom"></param>
    void SetZoom(double zoom);
}