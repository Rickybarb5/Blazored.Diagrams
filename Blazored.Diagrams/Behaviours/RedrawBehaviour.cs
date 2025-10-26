using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Some UI changes don't happen if they come from an external component.
///     This behaviour forces triggers the redraw event when necessary.
/// </summary>
public class RedrawBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;

    /// <summary>
    /// Instantiates a new RedrawBehaviour{T}
    /// </summary>
    /// <param name="service">Model to be redrawn.</param>
    public RedrawBehaviour(IDiagramService service)
    {
        _service = service;
        var options = _service.Behaviours.GetBehaviourOptions<RedrawBehaviourOptions>();
        options.OnEnabledChanged.Subscribe(OnEnabledChanged);
        OnEnabledChanged(options.IsEnabled);
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
            _service.Events.SubscribeTo<NodeAddedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<NodeRemovedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<NodeSizeChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<NodePositionChangedEvent>(e => NotifyRedraw(e.Model)),

            _service.Events.SubscribeTo<GroupAddedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<GroupRemovedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<GroupSizeChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<GroupPositionChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<GroupPaddingChangedEvent>(e => NotifyRedraw(e.Model)),

            _service.Events.SubscribeTo<PortAddedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<PortAddedToGroupEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<PortAddedToNodeEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<PortRemovedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<PortSizeChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<PortPositionChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<PortJustificationChangedEvent>(e => NotifyRedraw(e.Model)),

            _service.Events.SubscribeTo<LinkAddedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<LinkRemovedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<LinkSourcePortChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<LinkTargetPortChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<LinkTargetPositionChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<DiagramRedrawEvent>(RedrawFullDiagram),
        ];
    }

    private void RedrawFullDiagram(DiagramRedrawEvent ev)
    {
        ev.Model.Layers.ForEach(l=> _service.Events.Publish(new LayerRedrawEvent(l)));
        ev.Model.AllNodes.ForEach(n=> _service.Events.Publish(new NodeRedrawEvent(n)));
        ev.Model.AllGroups.ForEach(g=> _service.Events.Publish(new GroupRedrawEvent(g)));
        ev.Model.AllPorts.ForEach(p=> _service.Events.Publish(new PortRedrawEvent(p)));
        ev.Model.AllLinks.ForEach(l=> _service.Events.Publish(new LinkRedrawEvent(l)));
    }

    private void NotifyRedraw(INode obj)
    {
        PublishEvent(new NodeRedrawEvent(obj));
    }

    private void NotifyRedraw(IGroup obj)
    {
        PublishEvent(new GroupRedrawEvent(obj));
    }

    private void NotifyRedraw(IPort obj)
    {
        PublishEvent(new PortRedrawEvent(obj));
    }

    private void NotifyRedraw(ILink obj)
    {
        PublishEvent(new LinkRedrawEvent(obj));
    }

    private void PublishEvent<TEvent>(TEvent @event) where TEvent : IEvent
    {
        _service.Events.Publish(@event);
    }
}