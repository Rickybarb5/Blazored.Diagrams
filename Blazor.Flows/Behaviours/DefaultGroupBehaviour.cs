using Blazor.Flows.Events;
using Blazor.Flows.Extensions;
using Blazor.Flows.Groups;
using Blazor.Flows.Nodes;
using Blazor.Flows.Options.Behaviours;
using Blazor.Flows.Services.Diagrams;

namespace Blazor.Flows.Behaviours;

/// <summary>
/// Default behaviour of all groups
/// </summary>
public class DefaultGroupBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;
    private IDisposable _sizeEventSubscription = null!;
    private IDisposable _childMovementEventSubscription = null!;
    private IDisposable _positionEventSubscription = null!;

    /// <summary>
    /// Instantiates a new <see cref="DefaultGroupBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public DefaultGroupBehaviour(IDiagramService service)
    {
        _service = service;
        var behaviourOptions = _service.Behaviours.GetBehaviourOptions<DefaultGroupBehaviourOptions>();
        behaviourOptions.OnEnabledChanged.Subscribe(OnEnabledChanged);
        OnEnabledChanged(behaviourOptions.IsEnabled);
    }

    private void OnEnabledChanged(BehaviourEnabledEvent ev)
    {
        OnEnabledChanged(ev.IsEnabled);
    }

    private void OnEnabledChanged(bool isEnabled)
    {
        if (isEnabled)
        {
            SubscribeToEvents();
        }
        else
        {
            DisposeSubscriptions();
        }
    }

    private void SubscribeToEvents()
    {
        Subscriptions =
        [
            _service.Events.SubscribeTo<GroupAddedToGroupEvent>(HandleGroupAddedToGroupEvent),

            // Resize and positioning
            _service.Events.SubscribeTo<NodeSizeChangedEvent>(e => ResizeFromNode(e.Model)),
            _service.Events.SubscribeTo<NodePositionChangedEvent>(e => ResizeFromNode(e.Model)),
            _service.Events.SubscribeTo<NodeAddedToGroupEvent>(e => Resize(e.Model)),
            _service.Events.SubscribeTo<NodeRemovedFromGroupEvent>(e => Resize(e.Model)),

            _service.Events.SubscribeTo<GroupAddedToGroupEvent>(e => Resize(e.ParentModel)),
            _service.Events.SubscribeTo<GroupRemovedFromGroupEvent>(e => Resize(e.ParentModel)),
            _service.Events.SubscribeTo<GroupPaddingChangedEvent>(e => ResizeFromGroup(e.Model)),
            _service.Events.SubscribeTo<GroupPaddingChangedEvent>(e => Resize(e.Model)),
        ];
        SubscribeToRecursiveEvents();
    }

    /// <summary>
    /// This behaviour manages all group positions and size.
    /// This means that the events will become recursive.
    /// To fix this we subscribe and unsubscribe accordingly.
    /// </summary>
    private void SubscribeToRecursiveEvents()
    {
        _childMovementEventSubscription = _service.Events.SubscribeTo<GroupPositionChangedEvent>(OnGroupPositionChanged);
        _positionEventSubscription =
            _service.Events.SubscribeTo<GroupPositionChangedEvent>(e => ResizeFromGroup(e.Model));
        _sizeEventSubscription = _service.Events.SubscribeTo<GroupSizeChangedEvent>(e => ResizeFromGroup(e.Model));
    }

    /// <summary>
    /// This behaviour manages all group positions and size.
    /// This means that the events will become recursive.
    /// To fix this we subscribe and unsubscribe accordingly.
    /// </summary>
    private void UnsubscribeFromRecursiveEvents()
    {
        _childMovementEventSubscription.Dispose();
        _positionEventSubscription.Dispose();
        _sizeEventSubscription.Dispose();
    }

    private void ResizeFromNode(INode node)
    {
        _service.Diagram.AllGroups
            .Where(x => x.AllNodes.Contains(node))
            .ForEach(Resize);
    }

    private void ResizeFromGroup(IGroup group)
    {
        _service.Diagram.AllGroups
            .Where(x => x.AllGroups.Contains(group))
            .ForEach(Resize);
    }


    private static void HandleGroupAddedToGroupEvent(GroupAddedToGroupEvent obj)
    {
        // A group cannot be added to itself
        if (obj.ParentModel.Id == obj.AddedGroup.Id && obj.ParentModel.AllGroups.Any(g => g.Id == obj.AddedGroup.Id))
            throw new InvalidOperationException("Cannot add group to itself.");
    }

    /// <summary>
    /// When a group moves, the children move with it.
    /// </summary>
    /// <param name="obj"></param>
    private void OnGroupPositionChanged(GroupPositionChangedEvent obj)
    {
        // Move child groups
        var xDiff = obj.Model.PositionX - obj.OldX;
        var yDiff = obj.Model.PositionY - obj.OldY;
        obj.Model.AllGroups.ForEach(g =>
        {
            g.SetPositionInternal(g.PositionX + xDiff, g.PositionY + yDiff);
            _service.Events.Publish(new GroupRedrawEvent(g));
        });
        obj.Model.AllNodes.ForEach(n =>
        {
            n.SetPosition(n.PositionX + xDiff, n.PositionY + yDiff);
            _service.Events.Publish(new NodeRedrawEvent(n));
        });
        obj.Model.AllPorts.ForEach(p =>
        {
            p.SetPosition(p.PositionX + xDiff, p.PositionY + yDiff);
            _service.Events.Publish(new PortRedrawEvent(p));
        });
    }

    private void Resize(IGroup group)
    {
        ChangeGroupSizeAndPosition(group);
    }

    private void ChangeGroupSizeAndPosition(IGroup group)
    {
        if (group.AllNodes.Count == 0 && group.AllGroups.Count == 0) return;

        var allBounds = 
            group.AllNodes.Select(n => n.GetBounds())
                .Concat(group.AllGroups.Select(g => g.GetBounds()))
                .ToList();

        var minX = (int)allBounds.Min(b => b.Left);
        var minY = (int)allBounds.Min(b => b.Top);
        var maxX = (int)allBounds.Max(b => b.Right);
        var maxY = (int)allBounds.Max(b => b.Bottom);
        
        var newPositionX = minX - group.Padding;
        var newPositionY = minY - group.Padding;
        
        // Size: The distance between Max and Min, adjusted by Padding
        var newWidth = maxX - minX + group.Padding * 2;
        var newHeight = maxY - minY + group.Padding * 2;

        UnsubscribeFromRecursiveEvents();
        group.SetPosition(newPositionX, newPositionY);
        group.SetSize(newWidth, newHeight);
        SubscribeToRecursiveEvents();

        // Redraw ports manually, as we are not triggering group events
        // group.Ports.ForEach(p => _service.Events.Publish(new PortRedrawEvent(p)));
        // _service.Events.Publish(new GroupRedrawEvent(group));
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();
        UnsubscribeFromRecursiveEvents();
    }
}