using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Ports;

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
    public event Action<INode, int, int, int, int>? OnSizeChanged;

    /// <summary>
    /// Event triggered when the position changes
    /// </summary>
    public event Action<INode, int, int, int, int>? OnPositionChanged;

    /// <summary>
    /// Event triggered when the selection state changes.
    /// </summary>
    public event Action<INode>? OnSelectionChanged;

    /// <summary>
    /// EventTriggered when the visibility state changes
    /// </summary>
    public event Action<INode>? OnVisibilityChanged;

    /// <summary>
    ///     Event triggered when a port is added.
    /// </summary>
    public event Action<INode, IPort> OnPortAdded;

    /// <summary>
    ///     Event triggered when a port is removed.
    /// </summary>
    public event Action<INode, IPort> OnPortRemoved;
}