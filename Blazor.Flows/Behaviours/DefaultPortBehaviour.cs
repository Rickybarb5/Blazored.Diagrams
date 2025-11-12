using Blazor.Flows.Events;
using Blazor.Flows.Extensions;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Options.Behaviours;
using Blazor.Flows.Ports;
using Blazor.Flows.Services.Diagrams;

namespace Blazor.Flows.Behaviours;

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
            _service.Events.SubscribeTo<PortAddedToNodeEvent>(e=>AddParentToPort(e.Model, e.Port)),
            _service.Events.SubscribeTo<PortAddedToGroupEvent>(e=>AddParentToPort(e.Model, e.Port)),
            
            //Position management
            _service.Events.SubscribeTo<PortRedrawEvent>(e => RefreshPositionCoordinates(e.Model)),
            _service.Events.SubscribeTo<PortSizeChangedEvent>(e => RefreshPositionCoordinates(e.Model)),
            _service.Events.SubscribeTo<PortJustificationChangedEvent>(e => RefreshPositionCoordinates(e.Model)),
            _service.Events.SubscribeTo<PortAlignmentChangedEvent>(e => RefreshPositionCoordinates(e.Model)),
            _service.Events.SubscribeTo<GroupPositionChangedEvent>(e => RefreshPositionCoordinates(e.Model)),
            _service.Events.SubscribeTo<GroupPaddingChangedEvent>(e => RefreshPositionCoordinates(e.Model)),
            _service.Events.SubscribeTo<GroupSizeChangedEvent>(e => RefreshPositionCoordinates(e.Model)),
            _service.Events.SubscribeTo<NodePositionChangedEvent>(e => RefreshPositionCoordinates(e.Model)),
            _service.Events.SubscribeTo<NodeSizeChangedEvent>(e => RefreshPositionCoordinates(e.Model)),
        ];
    }

    private void AddParentToPort(IPortContainer portParent, IPort port)
    {
        port.Parent = portParent;
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

    private void RefreshPositionCoordinates(IPort port)
    {
        var newPosition = CalculatePosition(port);
        port.SetPosition(newPosition.PositionX, newPosition.PositionY);
    }

    private (int PositionX, int PositionY) CalculatePosition(IPort port)
    {
        var (x, y) = (Position: port.Alignment, Alignment: port.Justification) switch
        {
            (PortAlignment.Left, PortJustification.Start) => (port.Parent.PositionX - port.Width / 2,
                port.Parent.PositionY - port.Height / 2),
            (PortAlignment.Left, PortJustification.Center) => (port.Parent.PositionX - port.Width / 2,
                port.Parent.PositionY + port.Parent.Height / 2 - port.Height / 2),
            (PortAlignment.Left, PortJustification.End) => (port.Parent.PositionX - port.Width / 2,
                port.Parent.PositionY + port.Parent.Height - port.Height / 2),

            (PortAlignment.Right, PortJustification.Start) => (
                port.Parent.PositionX + port.Parent.Width - port.Width / 2,
                port.Parent.PositionY - port.Height / 2),
            (PortAlignment.Right, PortJustification.Center) => (
                port.Parent.PositionX + port.Parent.Width - port.Width / 2,
                port.Parent.PositionY + port.Parent.Height / 2 - port.Height / 2),
            (PortAlignment.Right, PortJustification.End) => (port.Parent.PositionX + port.Parent.Width - port.Width / 2,
                port.Parent.PositionY + port.Parent.Height - port.Height / 2),

            (PortAlignment.Top, PortJustification.Start) =>
                (port.Parent.PositionX - port.Width / 2, port.Parent.PositionY - port.Height / 2),
            (PortAlignment.Top, PortJustification.Center) => (
                port.Parent.PositionX + port.Parent.Width / 2 - port.Width / 2,
                port.Parent.PositionY - port.Height / 2),
            (PortAlignment.Top, PortJustification.End) => (port.Parent.PositionX + port.Parent.Width - port.Width / 2,
                port.Parent.PositionY - port.Height / 2),

            (PortAlignment.Bottom, PortJustification.Start) => (port.Parent.PositionX - port.Width / 2,
                port.Parent.PositionY + port.Parent.Height - port.Height / 2),
            (PortAlignment.Bottom, PortJustification.Center) => (
                port.Parent.PositionX + port.Parent.Width / 2 - port.Width / 2,
                port.Parent.PositionY + port.Parent.Height - port.Height / 2),
            (PortAlignment.Bottom, PortJustification.End) => (
                port.Parent.PositionX + port.Parent.Width - port.Width / 2,
                port.Parent.PositionY + port.Parent.Height - port.Height / 2),
            (PortAlignment.CenterParent, _) => (
                port.Parent.PositionX + port.Parent.Width / 2 - port.Width / 2,
                port.Parent.PositionY + port.Parent.Height / 2 - port.Height / 2),
            (PortAlignment.Custom, _) => port.CustomPositioning(),
            _ => (port.PositionX, port.PositionY)
        };
        return (x, y);
    }
    
    private void RefreshPositionCoordinates(IPortContainer container)
    {
        container.Ports.ForEach(RefreshPositionCoordinates);
    }
}