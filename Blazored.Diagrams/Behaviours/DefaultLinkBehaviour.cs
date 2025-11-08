using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Standard link behaviour.
/// </summary>
public class DefaultLinkBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;
    private readonly DefaultLinkBehaviourOptions _behaviourOptions;

    /// <summary>
    /// Instantiates a new <see cref="DefaultLinkBehaviour"/>
    /// </summary>
    public DefaultLinkBehaviour(IDiagramService service)
    {
        _service = service;
        _behaviourOptions = _service.Behaviours.GetBehaviourOptions<DefaultLinkBehaviourOptions>()!;
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
            _service.Events.SubscribeTo<LinkTargetPortChangedEvent>(HandleTargetPortChanged),
            _service.Events.SubscribeTo<LinkSourcePortChangedEvent>(HandleSourcePortConnected),
            _service.Events.SubscribeWhere<PortPositionChangedEvent>(p => p.Model.HasLinks,
                HandlePortEvent),
            _service.Events.SubscribeWhere<PortSizeChangedEvent>(p => p.Model.HasLinks, HandlePortEvent),
            _service.Events.SubscribeWhere<PortAlignmentChangedEvent>(p => p.Model.HasLinks,
                HandlePortEvent),
            _service.Events.SubscribeWhere<PortJustificationChangedEvent>(p => p.Model.HasLinks,
                HandlePortEvent),
        ];
    }

    private void HandlePortEvent(IModelEvent<IPort> obj)
    {
        UpdateLinkPosition(obj.Model);
    }

    private void HandleSourcePortConnected(LinkSourcePortChangedEvent obj)
    {
        obj.OldSourcePort?.OutgoingLinks.RemoveInternal(obj.Model);
        obj.NewSourcePort.OutgoingLinks.AddInternal(obj.Model);
    }

    private void HandleTargetPortChanged(LinkTargetPortChangedEvent obj)
    {
        obj.OldTargetPort?.IncomingLinks.RemoveInternal(obj.Model);
        obj.NewTargetPort?.IncomingLinks.AddInternal(obj.Model);
        if (obj.NewTargetPort is not null)
        {
            var centerCoordinates = _service.GetCenterCoordinates(obj.NewTargetPort);
            obj.Model.SetTargetPosition(centerCoordinates.CenterX, centerCoordinates.CenterY);
        }
    }

    private void UpdateLinkPosition(IPort port)
    {
        port.IncomingLinks.ForEach(l =>
        {
            if (l.TargetPort is not null)
            {
                var centerCoordinates = _service.GetCenterCoordinates(l.TargetPort);
                l.SetTargetPosition(centerCoordinates.CenterX, centerCoordinates.CenterY);
            }
        });

        port.OutgoingLinks.ForEach(l =>
        {
            var centerCoordinates = _service.GetCenterCoordinates(l.SourcePort);
            l.SetTargetPosition(centerCoordinates.CenterX, centerCoordinates.CenterY);
        });
    }
}