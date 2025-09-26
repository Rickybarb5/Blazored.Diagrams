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
        _service.Diagram.Layers.ForEach(SubscribeToEvents);
        _service.Diagram.AllNodes.ForEach(SubscribeToEvents);
        _service.Diagram.AllGroups.ForEach(SubscribeToEvents);
        _service.Diagram.AllPorts.ForEach(SubscribeToEvents);
        _service.Diagram.AllLinks.ForEach(SubscribeToEvents);
        
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
        _service.Diagram.Layers.ForEach(UnsubscribeFromEvents);
        _service.Diagram.AllNodes.ForEach(UnsubscribeFromEvents);
        _service.Diagram.AllGroups.ForEach(UnsubscribeFromEvents);
        _service.Diagram.AllPorts.ForEach(UnsubscribeFromEvents);
        _service.Diagram.AllLinks.ForEach(UnsubscribeFromEvents);
        _subscriptions.DisposeAll();
    }
}