using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Allows the user to move selected nodes and groups.
/// </summary>
public class MoveBehaviour : IBehaviour
{
    private readonly IDiagramService _service;
    private readonly MoveOptions _options;
    private bool _isDragging;
    private double _lastPointerX;
    private double _lastPointerY;
    private List<IDisposable> _subscriptions = [];

    /// <summary>
    /// Instantiates a new <see cref="MoveBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public MoveBehaviour(IDiagramService service)
    {
        _service = service;
        _options = _service.Diagram.Options.Get<MoveOptions>()!;
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

    private IEnumerable<IPosition> SelectedModels =>
        _service.Diagram
            .Layers
            .SelectMany(l => l.AllNodes.Where(x => x.IsSelected)
                .Cast<IPosition>()
                .Concat(l.AllGroups.Where(g => g.IsSelected)));

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
            _service.Events.SubscribeTo<NodePointerDownEvent>(OnPointerDown),
            _service.Events.SubscribeTo<GroupPointerDownEvent>(OnPointerDown),
            _service.Events.SubscribeTo<DiagramPointerMoveEvent>(OnPointerMove),
            _service.Events.SubscribeTo<DiagramPointerUpEvent>(OnPointerUp),
        ];
    }

    private void OnPointerUp(DiagramPointerUpEvent obj)
    {
        _isDragging = false;
        _lastPointerX = 0;
        _lastPointerY = 0;
    }

    private void OnPointerMove(DiagramPointerMoveEvent obj)
    {
        if (!_isDragging) return;

        var xDiff = obj.Args.ClientX - _lastPointerX;
        var yDiff = obj.Args.ClientY - _lastPointerY;

        // Apply inverse zoom to the movement
        var inverseZoom = 1 / obj.Model.Zoom;
        var xDiffAdjusted = xDiff * inverseZoom;
        var yDiffAdjusted = yDiff * inverseZoom;

        foreach (var model in SelectedModels)
        {
            var x = (int)(model.PositionX + xDiffAdjusted);
            var y = (int)(model.PositionY + yDiffAdjusted);
            model.SetPosition(x, y);
        }

        _lastPointerX = obj.Args.ClientX;
        _lastPointerY = obj.Args.ClientY;
    }

    private void OnPointerDown(GroupPointerDownEvent obj)
    {
        OnPointerDown(obj.Args);
    }

    private void OnPointerDown(NodePointerDownEvent obj)
    {
        OnPointerDown(obj.Args);
    }

    private void OnPointerDown(PointerEventArgs e)
    {
        _isDragging = true;
        _lastPointerX = e.ClientX;
        _lastPointerY = e.ClientY;
    }
}