using Blazored.Diagrams.Events;
using Blazored.Diagrams.Interfaces;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Nodes;

/// <summary>
///     Base interface to implement a base node class.
/// </summary>
public interface INode :
    IId,
    IVisible,
    ISelectable,
    IPortContainer,
    IDisposable
{
    /// <summary>
    ///     Sets the X and Y coordinates on the screen.
    ///     Does not trigger the event.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void SetPositionInternal(int x, int y);

    /// <summary>
    ///     Sets the width and height.
    ///     Does not trigger the event.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    void SetSizeInternal(int width, int height);

    /// <summary>
    /// Event triggered when the size changes.
    /// </summary>
    [JsonIgnore]
public ITypedEvent<NodeSizeChangedEvent> OnSizeChanged { get; init; }

    /// <summary>
    /// Event triggered when the position changes
    /// </summary>
    [JsonIgnore]
public ITypedEvent<NodePositionChangedEvent> OnPositionChanged { get; init; }

    /// <summary>
    /// Event triggered when the selection state changes.
    /// </summary>
    [JsonIgnore]
public ITypedEvent<NodeSelectionChangedEvent> OnSelectionChanged { get; init; }

    /// <summary>
    /// EventTriggered when the visibility state changes
    /// </summary>
    [JsonIgnore]
public ITypedEvent<NodeVisibilityChangedEvent> OnVisibilityChanged { get; init; }

    /// <summary>
    ///     Event triggered when a port is added.
    /// </summary>
    [JsonIgnore]
public ITypedEvent<PortAddedEvent> OnPortAdded { get; init; } 
    
    /// <summary>
    ///     Event triggered when a port is removed.
    /// </summary>
    [JsonIgnore]
public ITypedEvent<PortRemovedEvent> OnPortRemoved { get; init; } 
    
    /// <summary>
    ///     Event triggered when a port is added to this node.
    /// </summary>
    [JsonIgnore]
public ITypedEvent<PortAddedToNodeEvent> OnPortAddedToNode { get; init; } 

    /// <summary>
    ///     Event triggered when a port is removed from this node
    /// </summary>
    [JsonIgnore]
public ITypedEvent<PortRemovedFromNodeEvent> OnPortRemovedFromNode { get; init; }
}