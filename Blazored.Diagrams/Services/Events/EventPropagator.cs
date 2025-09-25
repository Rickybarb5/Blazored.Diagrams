using Blazored.Diagrams.Extensions;

namespace Blazored.Diagrams.Services.Events;

internal partial class EventPropagator : IEventPropagator
{
    private readonly IDiagramService _service;
    private List<IDisposable> _subscriptions = [];

    public EventPropagator(IDiagramService service)
    {
        _service = service;
        Initialize();
    }

    private void Initialize()
    {
        _service.Diagram.Layers.OnItemAdded += SubscribeToEvents;
        _service.Diagram.Layers.OnItemRemoved += UnsubscribeFromEvents;
        foreach (var layer in _service.Diagram.Layers)
        {
            SubscribeToEvents(layer);
        }

        foreach (var node in _service.Diagram.AllNodes)
        {
            SubscribeToEvents(node);
        }

        foreach (var group in _service.Diagram.AllGroups)
        {
            SubscribeToEvents(group);
        }

        foreach (var port in _service.Diagram.AllPorts)
        {
            SubscribeToEvents(port);
        }

        foreach (var link in _service.Diagram.AllLinks)
        {
            SubscribeToEvents(link);
        }

        SubscribeToEvents(_service.Diagram);

        _subscriptions =
        [
            //Subscribe/Unsubscribe to all new models.
            _service.Events.SubscribeTo<NodeAddedEvent>(SubscribeToEvents),
            _service.Events.SubscribeTo<GroupAddedEvent>(SubscribeToEvents),
            _service.Events.SubscribeTo<PortAddedEvent>(SubscribeToEvents),
            _service.Events.SubscribeTo<LinkAddedEvent>(SubscribeToEvents),
            _service.Events.SubscribeTo<LayerAddedEvent>(SubscribeToEvents),
            _service.Events.SubscribeTo<LayerRemovedEvent>(UnsubscribeFromEvents),
            _service.Events.SubscribeTo<NodeRemovedEvent>(UnsubscribeFromEvents),
            _service.Events.SubscribeTo<GroupRemovedEvent>(UnsubscribeFromEvents),
            _service.Events.SubscribeTo<PortRemovedEvent>(UnsubscribeFromEvents),
            _service.Events.SubscribeTo<LinkRemovedEvent>(UnsubscribeFromEvents),
        ];
    }
    
    public void Dispose()
    {
        _service.Diagram.Layers.OnItemAdded -= SubscribeToEvents;
        _service.Diagram.Layers.OnItemRemoved -= UnsubscribeFromEvents;
        foreach (var layer in _service.Diagram.Layers)
        {
            UnsubscribeFromEvents(layer);
        }

        foreach (var node in _service.Diagram.AllNodes)
        {
            UnsubscribeFromEvents(node);
        }

        foreach (var group in _service.Diagram.AllGroups)
        {
            UnsubscribeFromEvents(group);
        }

        foreach (var port in _service.Diagram.AllPorts)
        {
            UnsubscribeFromEvents(port);
        }

        foreach (var link in _service.Diagram.AllLinks)
        {
            UnsubscribeFromEvents(link);
        }

        _subscriptions.DisposeAll();
    }
}