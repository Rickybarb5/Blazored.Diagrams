using Blazored.Diagrams.Events;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Behaviour for panning the diagram using pointer click and drag.
/// </summary>
public class PanBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;
    private readonly PanBehaviourOptions _behaviourOptions;
    private bool _isPanning;
    private double _lastPointerX;
    private double _lastPointerY;

    /// <summary>
    /// Instantiates a new <see cref="PanBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public PanBehaviour(IDiagramService service)
    {
        _service = service;
        _behaviourOptions = _service.Behaviours.GetBehaviourOptions<PanBehaviourOptions>()!;
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
            _service.Events.SubscribeTo<DiagramPointerDownEvent>(OnPanStart),
            _service.Events.SubscribeTo<DiagramPointerMoveEvent>(OnPan),
            _service.Events.SubscribeTo<DiagramPointerUpEvent>(OnPanEnd),
        ];
    }

    private void OnPanStart(DiagramPointerDownEvent ev)
    {
        // We only pan if the ctrl key is not being pressed.
        // This is because of the multi select behaviour.
        if (!ev.Args.CtrlKey)
        {
            _isPanning = true;
            _lastPointerX = ev.Args.ClientX;
            _lastPointerY = ev.Args.ClientY;
            _service.Events.Publish(new PanStartEvent(ev.Model, ev.Model.PanX, ev.Model.PanY));
        }
    }

    private void OnPan(DiagramPointerMoveEvent ev)
    {
        if (_isPanning && _behaviourOptions.IsEnabled)
        {
            // Calculate the pointer movement delta
            var deltaX = ev.Args.ClientX - _lastPointerX;
            var deltaY = ev.Args.ClientY - _lastPointerY;

            var panX = (int)(_service.Diagram.PanX + deltaX);
            var panY = (int)(_service.Diagram.PanY + deltaY);

            _service.Diagram.SetPan(panX, panY);

            // Store the current pointer coordinates for the next pan event
            _lastPointerX = ev.Args.ClientX;
            _lastPointerY = ev.Args.ClientY;
        }
    }

    private void OnPanEnd(DiagramPointerUpEvent ev)
    {
        if (_isPanning)
        {
            _isPanning = false;
            _lastPointerX = 0;
            _lastPointerY = 0;
            _service.Events.Publish(new PanEndEvent(ev.Model, ev.Model.PanX, ev.Model.PanY));
        }
    }
}