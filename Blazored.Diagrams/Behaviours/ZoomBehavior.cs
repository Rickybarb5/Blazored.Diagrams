using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Behaviour for zooming the diagram using the pointer wheel.
/// </summary>
public class ZoomBehavior : BaseBehaviour
{
    private readonly IDiagramService _diagramService;
    private readonly ZoomBehaviourOptions _behaviourOptions;

    /// <summary>
    /// Instantiates a new <see cref="ZoomBehavior"/>
    /// </summary>
    /// <param name="diagramService"></param>
    public ZoomBehavior(IDiagramService diagramService)
    {
        _diagramService = diagramService;
        _behaviourOptions = _diagramService.Behaviours.GetBehaviourOptions<ZoomBehaviourOptions>()!;
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
            _diagramService.Events.SubscribeTo<DiagramWheelEvent>(e => StepZoom(e.Model, e.Args)),
            _diagramService.Events.SubscribeTo<DiagramZoomChangedEvent>(e => SetZoom(e.Model.Zoom))
            
        ];
    }

    private void StepZoom(IDiagram diagram, WheelEventArgs args)
    {
        if (!_behaviourOptions.IsEnabled) return;
        var wheelDeltaY = args.DeltaY;
        switch (wheelDeltaY)
        {
            case > 0:
                SetZoom(diagram.Zoom - _behaviourOptions.ZoomStep);
                break;
            case < 0:
                SetZoom(diagram.Zoom + _behaviourOptions.ZoomStep);
                break;
        }
    }
    
    private void SetZoom(double zoom)
    {
        
        if (zoom > _behaviourOptions.MaxZoom)
        {
            zoom = _behaviourOptions.MaxZoom;
        }
        else if (zoom < _behaviourOptions.MinZoom)
        {
            zoom = _behaviourOptions.MinZoom;
        }

        _diagramService.Diagram.Zoom = zoom;
        _diagramService.Events.Publish(new DiagramRedrawEvent(_diagramService.Diagram));
    }
}