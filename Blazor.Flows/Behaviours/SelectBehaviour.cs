using Blazor.Flows.Diagrams;
using Blazor.Flows.Events;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Options.Behaviours;
using Blazor.Flows.Services.Diagrams;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Flows.Behaviours;

/// <summary>
///     Selection behaviour for the UI.
///     Supports multi selection using ctrl key.
/// </summary>
public class SelectBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;
    private readonly SelectBehaviourOptions _behaviourOptions;

    /// <summary>
    /// Instantiates a new <see cref="SelectBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public SelectBehaviour(IDiagramService service)
    {
        _service = service;
        _behaviourOptions = _service.Behaviours.GetBehaviourOptions<SelectBehaviourOptions>()!;
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

    private void SelectModel(ISelectable selectableModel, PointerEventArgs args)
    {
        if (args.Button != 0 || !_behaviourOptions.SelectionEnabled) return;

        if (_behaviourOptions.MultiSelectEnabled && args.CtrlKey)
        {
            // Toggle selection when Ctrl is pressed
            selectableModel.IsSelected = !selectableModel.IsSelected;
        }
        else
        {
            _service.UnselectAll();
            selectableModel.IsSelected = true;
        }
    }

    private void HandleBackgroundClick(IDiagram diagram, PointerEventArgs args)
    {
        if (args.Button == 0) // Only handle left clicks
        {
            // Only clear selection if Ctrl is not pressed or multiselect is disabled
            if (!_behaviourOptions.MultiSelectEnabled || !args.CtrlKey)
            {
                _service.UnselectAll();
            }
        }
    }
    
    private void SubscribeToEvents()
    {
        Subscriptions =
        [
            _service.Events.SubscribeTo<NodePointerDownEvent>(e => SelectModel(e.Model, e.Args)),
            _service.Events.SubscribeTo<PortPointerDownEvent>(e => SelectModel(e.Model, e.Args)),
            _service.Events.SubscribeTo<GroupPointerDownEvent>(e => SelectModel(e.Model, e.Args)),
            _service.Events.SubscribeTo<LinkPointerDownEvent>(e => SelectModel(e.Model, e.Args)),
            _service.Events.SubscribeTo<DiagramPointerDownEvent>(e => HandleBackgroundClick(e.Model, e.Args)),
        ];
    }
}