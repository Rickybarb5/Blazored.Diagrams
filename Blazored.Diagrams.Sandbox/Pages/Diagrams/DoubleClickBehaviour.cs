using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Sandbox.Pages.Diagrams;

public class DoubleTapSelectBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;

    public DoubleTapSelectBehaviour(IDiagramService service)
    {
        _service = service;
        var opts = _service.Behaviours.GetBehaviourOptions<DoubleTapSelectOptions>()!;
        opts.OnEnabledChanged.Subscribe(OnEnabledChanged);
        OnEnabledChanged(opts.IsEnabled);
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
        // Subscribe to the Diagram Double Click Event
        Subscriptions =
        [
            _service.Events.SubscribeTo<DiagramDoubleClickedEvent>(HandleDiagramDoubleClicked)
        ];
    }

    private void HandleDiagramDoubleClicked(DiagramDoubleClickedEvent e)
    {
        // On double-click, we want to select ALL nodes.
        // This overrides any default double-click behaviour (like pan/zoom)
        // or provides custom logic.
        
        var diagram = e.Model;

        if (diagram.CurrentLayer.Nodes.Any())
        {
            diagram.CurrentLayer.Nodes.ForEach(n => n.IsSelected = true);
            
            _service.Events.Publish(new DiagramRedrawEvent(diagram));
        }
        else
        {
            diagram.UnselectAll();
        }
    }
}