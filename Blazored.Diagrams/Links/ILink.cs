using Blazored.Diagrams.Ports;
using System.Text.Json.Serialization;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Links;

/// <summary>
/// A link is a connection between two ports.
/// </summary>
public interface ILink : IId,
    ISelectable,
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
    /// Sets the x and y position of the target position to the center of the target port.
    /// </summary>
    void SetTargetPosition<TModel>(TModel model) where TModel : IPosition, ISize;

    /// <summary>
    /// Event triggered when the link container size changes.
    /// </summary>
    public event Action<ILink, int, int, int, int>? OnSizeChanged;

    /// <summary>
    ///     Event triggered when the target port is set.
    /// </summary>
    public event Action<ILink, IPort?, IPort?> OnTargetPortChanged;

    /// <summary>
    ///     Event triggered when the source port is set.
    /// </summary>
    public event Action<ILink, IPort, IPort> OnSourcePortChanged;

    /// <summary>
    ///     Event triggered when the target position of the link changes.
    /// </summary>
    public event Action<ILink, int, int, int, int> OnTargetPositionChanged;

    /// <summary>
    /// Event triggered when the <see cref="ISelectable.IsSelected"/> flag changes.
    /// </summary>
    public event Action<ILink>? OnSelectionChanged;

    /// <summary>
    /// Event triggered when the <see cref="IVisible.IsVisible"/> flag changes.
    /// </summary>
    public event Action<ILink>? OnVisibilityChanged;
}