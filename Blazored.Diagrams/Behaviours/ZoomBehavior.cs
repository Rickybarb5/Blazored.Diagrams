using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;
using Blazored.Diagrams.Services.Events;
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
        _behaviourOptions.OnEnabledChanged += OnEnabledChanged;
        OnEnabledChanged(_behaviourOptions.IsEnabled);
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

    private new void DisposeSubscriptions()
    {
        Subscriptions.DisposeAll();
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
        if (zoom == _diagramService.Diagram.Zoom) return;
        
        if (zoom > _behaviourOptions.MaxZoom)
        {
            zoom = _behaviourOptions.MaxZoom;
        }
        else if (zoom < _behaviourOptions.MinZoom)
        {
            zoom = _behaviourOptions.MinZoom;
        }

        _diagramService.Diagram.Zoom = zoom;
    }
}