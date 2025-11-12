using Blazor.Flows.Groups;
using Blazor.Flows.Layers;
using Blazor.Flows.Nodes;

namespace Blazor.Flows.Events;

// Layer Events

/// <summary>
/// Event triggered when a new layer will start to be used.
/// </summary>
/// <param name="OldLayer">Previous layer that was being used.</param>
/// <param name="NewLayer">New layer that will be used.</param>
public record CurrentLayerChangedEvent(ILayer OldLayer, ILayer NewLayer) : IEvent;

/// <summary>
/// Base layer event.
/// </summary>
/// <param name="Model">Layer that triggered the event</param>
public record LayerEvent(ILayer Model) : ModelEventBase<ILayer>(Model);

/// <summary>
/// Event triggered when a layer is added to the diagram.
/// </summary>
/// <param name="Model">The <see cref="ILayer"/> that was added.</param>
public record LayerAddedEvent(ILayer Model) : LayerEvent(Model);

/// <summary>
/// Event triggered when a layer is removed from the diagram.
/// </summary>
/// <param name="Model">The <see cref="ILayer"/> that was removed.</param>
public record LayerRemovedEvent(ILayer Model) : LayerEvent(Model);

/// <summary>
/// Event triggered when the visibility of a layer changes.
/// </summary>
/// <param name="Model">The <see cref="ILayer"/> whose visibility state changed.</param>
public record LayerVisibilityChangedEvent(ILayer Model) : LayerEvent(Model);

/// <summary>
/// Event triggered when a redraw is requested for a layer.
/// </summary>
/// <param name="Model">The <see cref="ILayer"/> that requested the redraw.</param>
public record LayerRedrawEvent(ILayer Model) : LayerEvent(Model);

/// <summary>
/// Event triggered when a node is added to a layer's collection.
/// </summary>
/// <param name="Model">The <see cref="ILayer"/> the node was added to.</param>
/// <param name="Node">The <see cref="INode"/> that was added.</param>
public record NodeAddedToLayerEvent(ILayer Model, INode Node) : LayerEvent(Model);

/// <summary>
/// Event triggered when a node is removed from a layer's collection.
/// </summary>
/// <param name="Model">The <see cref="ILayer"/> the node was removed from.</param>
/// <param name="Node">The <see cref="INode"/> that was removed.</param>
public record NodeRemovedFromLayerEvent(ILayer Model, INode Node) : LayerEvent(Model);

/// <summary>
/// Event triggered when a group is added to a layer's collection.
/// </summary>
/// <param name="Model">The <see cref="ILayer"/> the group was added to.</param>
/// <param name="AddedGroup">The <see cref="IGroup"/> that was added.</param>
public record GroupAddedToLayerEvent(ILayer Model, IGroup AddedGroup) : LayerEvent(Model);

/// <summary>
/// Event triggered when a group is removed from the layer's collection.
/// </summary>
/// <param name="Model">The <see cref="ILayer"/> the group was removed from.</param>
/// <param name="RemovedGroup">The <see cref="IGroup"/> that was removed.</param>
public record GroupRemovedFromLayerEvent(ILayer Model, IGroup RemovedGroup) : LayerEvent(Model);

/// <summary>
///  Event triggered when the ZIndex of a Layer changes.
/// </summary>
/// <param name="Model">The <see cref="ILayer"/> whose ZIndex was changed.</param>
public record LayerZIndexChanged(ILayer Model) : LayerEvent(Model);