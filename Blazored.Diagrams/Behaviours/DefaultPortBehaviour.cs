using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// Default behaviour of all ports.
/// </summary>
public class DefaultPortBehaviour : IBehaviour
{
    private readonly IDiagramService _service;
    private readonly DefaultPortOptions _options;
    private List<IDisposable> _subscriptions;

    /// <summary>
    /// Instantiates a new <see cref="DefaultPortBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public DefaultPortBehaviour(IDiagramService service)
    {
        _service = service;
        _options = _service.Diagram.Options.Get<DefaultPortOptions>()!;
        _options.OnEnabledChanged += OnEnabledChanged;
        OnEnabledChanged(_options.IsEnabled);
    }

    private void OnEnabledChanged(bool enabled)
    {
        if (enabled)
        {
            SubscribeToEvents();
        }
        else
        {
            DisposeSubscriptions();
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
            // Change port position.
            _service.Events.SubscribeTo<PortJustificationChangedEvent>(HandleJustificationUpdate),
            _service.Events.SubscribeTo<PortAlignmentChangedEvent>(HandleAlignmentUpdate),
            _service.Events.SubscribeTo<PortRedrawEvent>(HandleRedraw),
            _service.Events.SubscribeWhere<GroupPositionChangedEvent>(p => p.Model.Ports.Count != 0,
                HandleParentPositionChange),
            _service.Events.SubscribeWhere<GroupSizeChangedEvent>(p => p.Model.Ports.Count != 0,
                HandleParentSizeChange),
            _service.Events.SubscribeWhere<NodePositionChangedEvent>(p => p.Model.Ports.Count != 0,
                HandleParentPositionChange),
            _service.Events.SubscribeWhere<NodeSizeChangedEvent>(p => p.Model.Ports.Count != 0,
                HandleParentSizeChange),

            // Link management
            _service.Events.SubscribeTo<IncomingLinkAddedEvent>(HandleIncomingLinkAdded),
            _service.Events.SubscribeTo<IncomingLinkRemovedEvent>(HandleIncomingLinkRemoved),
            _service.Events.SubscribeTo<OutgoingLinkAddedEvent>(HandleOutgoingLinkAdded),
            _service.Events.SubscribeTo<OutgoingLinkRemovedEvent>(HandleOutgoingLinkRemoved),

            //Parent management
            _service.Events.SubscribeTo<PortAddedToNodeEvent>(HandlePortAddedToNodeEvent),
            _service.Events.SubscribeTo<PortAddedToGroupEvent>(HandlePortAddedToGroupEvent),
        ];
    }

    private void HandleRedraw(PortRedrawEvent obj)
    {
        obj.Model.RefreshPositionCoordinates();
    }

    private void HandleParentSizeChange(GroupSizeChangedEvent obj)
    {
        obj.Model.Ports.ForEach(x => x.RefreshPositionCoordinates());
    }

    private void HandleParentPositionChange(GroupPositionChangedEvent obj)
    {
        obj.Model.Ports.ForEach(x => x.RefreshPositionCoordinates());
    }

    private void HandleParentSizeChange(NodeSizeChangedEvent obj)
    {
        obj.Model.Ports.ForEach(x => x.RefreshPositionCoordinates());
    }

    private void HandleParentPositionChange(NodePositionChangedEvent obj)
    {
        obj.Model.Ports.ForEach(x => x.RefreshPositionCoordinates());
    }

    private void HandleAlignmentUpdate(PortAlignmentChangedEvent ev)
    {
        ev.Model.RefreshPositionCoordinates();
    }

    private void HandleJustificationUpdate(PortJustificationChangedEvent obj)
    {
        obj.Model.RefreshPositionCoordinates();
    }

    private void HandlePortAddedToGroupEvent(PortAddedToGroupEvent obj)
    {
        obj.Port.Parent = obj.Model;
    }

    private void HandlePortAddedToNodeEvent(PortAddedToNodeEvent obj)
    {
        obj.Port.Parent = obj.Model;
    }

    private void HandleOutgoingLinkRemoved(OutgoingLinkRemovedEvent obj)
    {
        // Link without a source port should not exist
        obj.RemovedLink.Dispose();
    }

    private void HandleOutgoingLinkAdded(OutgoingLinkAddedEvent obj)
    {
        obj.AddedLink.SourcePort = obj.Model;
    }

    private void HandleIncomingLinkRemoved(IncomingLinkRemovedEvent obj)
    {
        obj.RemovedLink.TargetPort = null;
    }

    private void HandleIncomingLinkAdded(IncomingLinkAddedEvent obj)
    {
        obj.AddedLink.TargetPort = obj.Model;
    }


    /// <inheritdoc />
    public void Dispose()
    {
        DisposeSubscriptions();
    }
}