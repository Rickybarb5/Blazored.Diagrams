using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Standard link behaviour.
/// </summary>
public class DefaultLinkBehaviour : IBehaviour
{
    private readonly IDiagramService _service;
    private readonly DefaultLinkOptions _options;
    private List<IDisposable> _subscriptions;

    /// <summary>
    /// Instantiates a new <see cref="DefaultLinkBehaviour"/>
    /// </summary>
    /// <param name="link"></param>
    public DefaultLinkBehaviour(IDiagramService service)
    {
        _service = service;
        _options = _service.Diagram.Options.Get<DefaultLinkOptions>()!;
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

    /// <inheritdoc />
    public void Dispose()
    {
        DisposeSubscriptions();
    }

    private void DisposeSubscriptions()
    {
        _subscriptions.DisposeAll();
    }

    private void SubscribeToEvents()
    {
        _subscriptions =
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
        obj.OldSourcePort.OutgoingLinks.Remove(obj.Model);
        obj.NewSourcePort.OutgoingLinks.Add(obj.Model);
    }

    private void HandleTargetPortChanged(LinkTargetPortChangedEvent obj)
    {
        obj.OldTargetPort?.IncomingLinks.Remove(obj.Model);
        obj.NewTargetPort?.IncomingLinks.Add(obj.Model);
        if (obj.NewTargetPort is not null)
        {
            obj.Model.SetTargetPosition(obj.NewTargetPort);
        }
    }

    private static void UpdateLinkPosition(IPort port)
    {
        port.IncomingLinks.ForEach(l =>
        {
            if (l.TargetPort is not null)
            {
                var centerCoordinates = l.TargetPort.GetCenterCoordinates();
                l.SetTargetPosition(centerCoordinates.CenterX, centerCoordinates.CenterY);
            }
        });

        port.OutgoingLinks.ForEach(l =>
        {
            var centerCoordinates = l.SourcePort.GetCenterCoordinates();
            l.SetTargetPosition(centerCoordinates.CenterX, centerCoordinates.CenterY);
        });
    }
}