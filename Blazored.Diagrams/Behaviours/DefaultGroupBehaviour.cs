using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// Default behaviour of all groups
/// </summary>
public class DefaultGroupBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;

    /// <summary>
    /// Instantiates a new <see cref="DefaultGroupBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public DefaultGroupBehaviour(IDiagramService service)
    {
        _service = service;
        var behaviourOptions = _service.Behaviours.GetBehaviourOptions<DefaultGroupBehaviourOptions>()!;
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
            _service.Events.SubscribeTo<GroupPositionChangedEvent>(OnGroupPositionChanged),
        ];
    }


    private static void HandleGroupAddedToGroupEvent(GroupAddedToGroupEvent obj)
    {
        // A group cannot be added to itself
        if (obj.ParentModel.Id == obj.AddedGroup.Id && obj.ParentModel.AllGroups.Any(g => g.Id == obj.AddedGroup.Id))
            throw new InvalidOperationException("Cannot add group to itself.");
    }

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
}