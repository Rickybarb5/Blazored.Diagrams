using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Behaviour for zooming the diagram using the pointer wheel.
/// </summary>
public class ZoomBehavior : IBehaviour
{
    private readonly IDiagramService _diagramService;
    private readonly ZoomOptions _options;
    private IDisposable _eventSubscription;

    /// <summary>
    /// Instantiates a new <see cref="ZoomBehavior"/>
    /// </summary>
    /// <param name="diagramService"></param>
    public ZoomBehavior(IDiagramService diagramService)
    {
        _diagramService = diagramService;
        _options = _diagramService.Diagram.Options.Get<ZoomOptions>()!;
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
        _eventSubscription.Dispose();
    }

    private void SubscribeToEvents()
    {
        _eventSubscription =
            _diagramService.Events.SubscribeTo<DiagramWheelEvent>(e => Events_OnWheel(e.Model, e.Args));
    }

    private void Events_OnWheel(IDiagram diagram, WheelEventArgs args)
    {
        if (!_options.IsEnabled) return;
        var wheelDeltaY = args.DeltaY;
        switch (wheelDeltaY)
        {
            case > 0:
                _diagramService.Diagram.StepZoomDown();
                break;
            case < 0:
                _diagramService.Diagram.StepZoomUp();
                break;
        }
    }
}