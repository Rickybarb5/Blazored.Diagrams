using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Some UI changes don't happen if they come from an external component.
///     This behaviour forces triggers the redraw event when necessary.
/// </summary>
/// <typeparam name="T"></typeparam>
public class RedrawBehaviour : IBehaviour
{
    private readonly IDiagramService _service;
    private List<IDisposable> _subscriptions = [];

    /// <summary>
    /// Instantiates a new RedrawBehaviour{T}
    /// </summary>
    /// <param name="service">Model to be redrawn.</param>
    public RedrawBehaviour(IDiagramService service)
    {
        _service = service;
        SubscribeToEvents();
    }


    /// <inheritdoc />
    public void Dispose()
    {
        DisposeSubscriptions();
    }

    private bool _isEnabled = true;


    /// <inheritdoc />
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (value == _isEnabled) return;
            _isEnabled = value;
            if (value)
            {
                SubscribeToEvents();
            }
            else
            {
                DisposeSubscriptions();
            }
        }
    }

    private void DisposeSubscriptions()
    {
        _subscriptions.DisposeAll();
    }

    private void SubscribeToEvents()
    {
        _subscriptions =
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
            _service.Events.SubscribeTo<PortRemovedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<PortSizeChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<PortPositionChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<PortJustificationChangedEvent>(e => NotifyRedraw(e.Model)),

            _service.Events.SubscribeTo<LinkAddedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<LinkRemovedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<LinkSourcePortChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<LinkTargetPortChangedEvent>(e => NotifyRedraw(e.Model)),
            _service.Events.SubscribeTo<LinkTargetPositionChangedEvent>(e => NotifyRedraw(e.Model)),
        ];
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