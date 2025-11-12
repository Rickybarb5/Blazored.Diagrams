using Blazor.Flows.Events;
using Blazor.Flows.Ports;

using Blazor.Flows.Interfaces;
using Newtonsoft.Json;

namespace Blazor.Flows.Links;

/// <summary>
/// A link is a connection between two ports.
/// </summary>
public interface ILink : IId,
    ISelectable,
    IDepth,
    IVisible,
    ISize,
    IDisposable
{
    /// <summary>
    ///     Port where the link originates from.
    /// </summary>
    [JsonIgnore]
    public IPort SourcePort { get; set; }

    /// <summary>
    ///     End port that the link connects to, if it exists.
    /// </summary>
    [JsonIgnore]
    public IPort? TargetPort { get; set; }

    /// <summary>
    ///     Gets a value indicating if the port is connected to a target port.
    /// </summary>
    public bool IsConnected { get; }
    
    /// <summary>
    ///     X coordinate of the target position of the end of the link.
    ///     If connected to a target port, it is the center of that element's position.
    /// </summary>
    public int TargetPositionX { get; set; }

    /// <summary>
    ///     Y coordinate of the target position of the end of the link.
    ///     If connected to a target port, it is the center of that element's position.
    /// </summary>
    public int TargetPositionY { get; set; }

    /// <summary>
    ///     Updates the end position of the link.
    /// </summary>
    /// <param name="x">X coordinate of the new posistion.</param>
    /// <param name="y">Y coordinate of the new position.</param>
    void SetTargetPosition(int x, int y);

    /// <summary>
    /// Event triggered when the link container size changes.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<LinkSizeChangedEvent> OnSizeChanged { get; init; }

    /// <summary>
    ///     Event triggered when the target port is set.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<LinkTargetPortChangedEvent> OnTargetPortChanged { get; init; }

    /// <summary>
    ///     Event triggered when the source port is set.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<LinkSourcePortChangedEvent> OnSourcePortChanged { get; init; }

    /// <summary>
    ///     Event triggered when the target position of the link changes.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<LinkTargetPositionChangedEvent> OnTargetPositionChanged { get; init; }

    /// <summary>
    /// Event triggered when the <see cref="ISelectable.IsSelected"/> flag changes.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<LinkSelectionChangedEvent> OnSelectionChanged { get; init; }

    /// <summary>
    /// Event triggered when the <see cref="IVisible.IsVisible"/> flag changes.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<LinkVisibilityChangedEvent> OnVisibilityChanged { get; init; }
    
    /// <summary>
    /// Event triggered when the <see cref="ILink.ZIndex"/> changes.
    /// </summary>
    [JsonIgnore]
    ITypedEvent<LinkZIndexChangedEvent> OnZIndexChanged { get; init; }
}