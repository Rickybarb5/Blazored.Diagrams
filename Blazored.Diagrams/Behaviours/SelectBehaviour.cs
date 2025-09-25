using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services;
using Blazored.Diagrams.Services.Events;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Selection behaviour for the UI.
///     Supports multi selection using ctrl key.
/// </summary>
public class SelectBehaviour : IBehaviour
{
    private readonly IDiagramService _service;
    private readonly SelectOptions _options;


    private List<IDisposable> _subscriptions = [];

    /// <summary>
    /// Instantiates a new <see cref="SelectBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public SelectBehaviour(IDiagramService service)
    {
        _service = service;
        _options = _service.Diagram.Options.Get<SelectOptions>()!;
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

    private void SelectModel(ISelectable selectableModel, PointerEventArgs args)
    {
        if (args.Button != 0 || !_options.SelectionEnabled) return;

        if (_options.MultiSelectEnabled && args.CtrlKey)
        {
            // Toggle selection when Ctrl is pressed
            selectableModel.IsSelected = !selectableModel.IsSelected;
        }
        else
        {
            // If not multiselect or Ctrl not pressed, unselect all and select the new one
            var wasSelected = selectableModel.IsSelected;
            _service.Diagram.UnselectAll();
            selectableModel.IsSelected = true;
        }
    }

    private void HandleBackgroundClick(IDiagram diagram, PointerEventArgs args)
    {
        if (args.Button == 0) // Only handle left clicks
        {
            // Only clear selection if Ctrl is not pressed or multiselect is disabled
            if (!_options.MultiSelectEnabled || !args.CtrlKey)
            {
                diagram.UnselectAll();
            }
        }
    }

    private void DisposeSubscriptions()
    {
        _subscriptions.DisposeAll();
    }

    private void SubscribeToEvents()
    {
        _subscriptions =
        [
            _service.Events.SubscribeTo<NodePointerDownEvent>(e => SelectModel(e.Model, e.Args)),
            _service.Events.SubscribeTo<GroupPointerDownEvent>(e => SelectModel(e.Model, e.Args)),
            _service.Events.SubscribeTo<LinkPointerDownEvent>(e => SelectModel(e.Model, e.Args)),
            _service.Events.SubscribeTo<DiagramPointerDownEvent>(e => HandleBackgroundClick(e.Model, e.Args)),
        ];
    }
}