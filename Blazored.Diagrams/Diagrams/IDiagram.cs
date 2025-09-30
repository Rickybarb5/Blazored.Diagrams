using System.Text.Json.Serialization;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Diagram;
using Blazored.Diagrams.Ports;

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
    IReadOnlyList<INode> AllNodes { get; }

    /// <summary>
    ///     Gets the links from all layers.
    /// </summary>
    IReadOnlyList<ILink> AllLinks { get; }

    /// <summary>
    ///     Gets the groups from all layers.
    /// </summary>
    IReadOnlyList<IGroup> AllGroups { get; }

    /// <summary>
    /// Gets all ports in the diagram.
    /// </summary>
    IReadOnlyList<IPort> AllPorts { get; }

    /// <summary>
    /// Gets the current active layer
    /// </summary>
    ILayer CurrentLayer { get; }

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
    DiagramOptions Options { get; init; }

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
    internal event Action<IDiagram, int, int, int, int> OnSizeChanged;

    /// <summary>
    /// Event triggered when the zoom value changes.
    /// </summary>
    event Action<IDiagram, double, double>? OnZoomChanged;

    /// <summary>
    /// Event triggered when the pan value changes.
    /// </summary>
    event Action<IDiagram, int, int, int, int>? OnPanChanged;

    /// <summary>
    /// Event triggered when a layer is added.
    /// </summary>
    event Action<IDiagram, ILayer>? OnLayerAdded;

    /// <summary>
    /// Event triggered when a layer is removed.
    /// </summary>
    event Action<IDiagram, ILayer>? OnLayerRemoved;

    /// <summary>
    /// Event triggered when the diagram position changes.
    /// </summary>
    event Action<IDiagram, int, int, int, int>? OnPositionChanged;

    /// <summary>
    /// Unselects all models in the diagram.
    /// </summary>
    void UnselectAll();

    /// <summary>
    /// Selects all models in the diagram.
    /// </summary>
    void SelectAll();

    /// <inheritdoc />
    void StepZoomUp();

    /// <inheritdoc />
    void StepZoomDown();

    /// <inheritdoc />
    void UseLayer(ILayer layer);

    /// <inheritdoc />
    void UseLayer(Guid layerId);
}