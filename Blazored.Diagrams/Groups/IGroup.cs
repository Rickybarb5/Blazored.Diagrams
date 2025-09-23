using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Groups;

/// <summary>
///     Interface to implement basic group features.
/// </summary>
public interface IGroup :
    IId,
    IVisible,
    INodeContainer,
    IGroupContainer,
    IPortContainer,
    ISelectable,
    IPadding,
    IDisposable
{
    /// <summary>
    ///     Gets all nodes and nested Nodes.
    /// </summary>
    IReadOnlyList<INode> AllNodes { get; }

    /// <summary>
    ///     Gets all groups and nested groups.
    /// </summary>
    IReadOnlyList<IGroup> AllGroups { get; }

    /// <summary>
    ///     Gets all ports and nested ports.
    /// </summary>
    IReadOnlyList<IPort> AllPorts { get; }

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
    public event Action<IGroup, int, int, int, int>? OnSizeChanged;

    /// <summary>
    /// Event triggered when the position changes
    /// </summary>
    public event Action<IGroup, int, int, int, int>? OnPositionChanged;

    /// <summary>
    /// Event triggered when the selection state changes.
    /// </summary>
    public event Action<IGroup>? OnSelectionChanged;

    /// <summary>
    /// EventTriggered when the visibility state changes
    /// </summary>
    public event Action<IGroup>? OnVisibilityChanged;


    /// <summary>
    ///     Event triggered when a node is added.
    /// </summary>
    event Action<IGroup, INode> OnNodeAdded;

    /// <summary>
    ///     Event triggered when a node is removed.
    /// </summary>
    event Action<IGroup, INode> OnNodeRemoved;

    /// <summary>
    ///     Event triggered when a nested group is added.
    /// </summary>
    event Action<IGroup, IGroup> OnGroupAdded;

    /// <summary>
    ///     Event triggered when a nested group is removed.
    /// </summary>
    event Action<IGroup, IGroup> OnGroupRemoved;

    /// <summary>
    ///     Event triggered when a port is added.
    /// </summary>
    public event Action<IGroup, IPort> OnPortAdded;

    /// <summary>
    ///     Event triggered when a port is removed.
    /// </summary>
    public event Action<IGroup, IPort> OnPortRemoved;

    /// <summary>
    /// Event triggered when the padding changes.
    /// </summary>
    event Action<IGroup, int, int> OnPaddingChanged;

    /// <summary>
    /// Unselects all models in a group
    /// </summary>
    void UnselectAll();

    /// <summary>
    /// Selects all models in a group.
    /// </summary>
    void SelectAll();
}