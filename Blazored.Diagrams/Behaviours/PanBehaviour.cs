using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Behaviour for panning the diagram using pointer click and drag.
/// </summary>
public class PanBehaviour : IBehaviour
{
    private readonly IDiagramService _service;
    private readonly PanOptions _options;
    private List<IDisposable> _subscriptions = [];
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
        _options = _service.Diagram.Options.Get<PanOptions>()!;
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
        _options.OnEnabledChanged -= OnEnabledChanged;
    }

    private void DisposeSubscriptions()
    {
        _subscriptions.DisposeAll();
    }

    private void SubscribeToEvents()
    {
        _subscriptions =
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
        if (_isPanning && _options.IsEnabled)
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