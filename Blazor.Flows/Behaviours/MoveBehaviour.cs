using Blazor.Flows.Events;
using Blazor.Flows.Extensions;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Options.Behaviours;
using Blazor.Flows.Services.Diagrams;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Flows.Behaviours;

/// <summary>
///     Allows the user to move selected nodes and groups.
/// </summary>
public class MoveBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;
    private readonly MoveBehaviourOptions _behaviourOptions;
    private bool _isDragging;
    private double _lastPointerX;
    private double _lastPointerY;

    /// <summary>
    /// Instantiates a new <see cref="MoveBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public MoveBehaviour(IDiagramService service)
    {
        _service = service;
        _behaviourOptions = _service.Behaviours.GetBehaviourOptions<MoveBehaviourOptions>()!;
        _behaviourOptions.OnEnabledChanged.Subscribe(OnEnabledChanged);
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

    private IEnumerable<IPosition> SelectedModels =>
        _service.Diagram
            .Layers
            .SelectMany(l => l.AllNodes.Where(x => x.IsSelected)
                .Cast<IPosition>()
                .Concat(l.AllGroups.Where(g => g.IsSelected)));

    private void SubscribeToEvents()
    {
        Subscriptions =
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

        SelectedModels.ForEach(model =>
        {
            var x = (int)(model.PositionX + xDiffAdjusted);
            var y = (int)(model.PositionY + yDiffAdjusted);
            model.SetPosition(x, y);
        });

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