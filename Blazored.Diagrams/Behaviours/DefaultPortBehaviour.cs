using Blazored.Diagrams.Events;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// Default behaviour of all ports.
/// </summary>
public class DefaultPortBehaviour : BaseBehaviour
{
    private readonly DefaultPortBehaviourOptions _behaviourOptions;
    private readonly IDiagramService _service;

    /// <summary>
    /// Instantiates a new <see cref="DefaultPortBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public DefaultPortBehaviour(IDiagramService service)
    {
        _service = service;
        _behaviourOptions = _service.Behaviours.GetBehaviourOptions<DefaultPortBehaviourOptions>()!;
        _behaviourOptions.OnEnabledChanged.Subscribe(OnEnabledChanged);
        OnEnabledChanged(_behaviourOptions.IsEnabled);
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
}