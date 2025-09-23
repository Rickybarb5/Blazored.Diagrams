using System.Text.Json.Serialization;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Layers;

/// <summary>
///     Contains all methods relevant for layers.
/// </summary>
public interface ILayer : IId,
    IVisible,
    INodeContainer,
    IGroupContainer,
    IDisposable
{
    /// <summary>
    /// Gets a value indicating if this layer is the one currently being used.
    /// </summary>
    public bool IsCurrentLayer { get; set; }

    /// <summary>
    ///     Ports that are container within the layer.
    /// </summary>
    [JsonIgnore]
    IReadOnlyList<IPort> AllPorts { get; }

    /// <summary>
    ///     All top-level groups and nested groups.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<IGroup> AllGroups { get; }

    /// <summary>
    ///     All top-level nodes and nested nodes.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<INode> AllNodes { get; }

    /// <summary>
    ///     All link contained within this layer.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<ILink> AllLinks { get; }

    /// <summary>
    /// Event triggered when the layer starts/stops being used.
    /// </summary>
    public event Action<ILayer>? OnLayerUsageChanged;

    /// <summary>
    /// Event triggered when the visibility state changes.
    /// </summary>
    public event Action<ILayer>? OnVisibilityChanged;

    /// <summary>
    ///     Event triggered when a node is added.
    /// </summary>
    event Action<ILayer, INode> OnNodeAdded;

    /// <summary>
    ///     Event triggered when a node is removed.
    /// </summary>
    event Action<ILayer, INode> OnNodeRemoved;

    /// <summary>
    ///     Event triggered when a nested group is added.
    /// </summary>
    event Action<ILayer, IGroup> OnGroupAdded;

    /// <summary>
    ///     Event triggered when a nested group is removed.
    /// </summary>
    event Action<ILayer, IGroup> OnGroupRemoved;

    /// <summary>
    /// Unselects all models in a layer
    /// </summary>
    void UnselectAll();

    /// <summary>
    /// Selects ll models in a layer
    /// </summary>
    void SelectAll();
}