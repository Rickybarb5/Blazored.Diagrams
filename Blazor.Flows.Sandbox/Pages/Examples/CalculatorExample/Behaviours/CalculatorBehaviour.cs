using Blazor.Flows.Behaviours;
using Blazor.Flows.Events;
using Blazor.Flows.Sandbox.Pages.Examples.CalculatorExample.Nodes;
using Blazor.Flows.Services.Diagrams;
using OperatorNodeEvents = Blazor.Flows.Sandbox.Pages.Examples.CalculatorExample.Nodes.OperatorNode; // ALIAS

namespace Blazor.Flows.Sandbox.Pages.Examples.CalculatorExample.Behaviours;

// Implementing IDisposable explicitly if BaseBehaviour doesn't, 
// otherwise rely on BaseBehaviour's implementation.
public class CalculatorBehaviour : BaseBehaviour
{
    private readonly IDiagramService _diagramService;
    
    public CalculatorBehaviour(IDiagramService diagramService)
    {
        _diagramService = diagramService;
        Subscriptions =
        [
            _diagramService.Events.SubscribeTo<NumberNodeChangedEvent>(HandleNumberChange),
            _diagramService.Events.SubscribeTo<OperatorNodeEvents.OperatorChangedEvent>(HandleOperatorChange),
            _diagramService.Events.SubscribeTo<OperatorNodeEvents.OperationResultChangedEvent>(HandleResultChange),
        ];
    }
    
    private void HandleResultChange(OperatorNodeEvents.OperationResultChangedEvent ev)
    {
        if (ev.Node.OutputPort.Target is { } target)
        {
            target.Calculate();
            _diagramService.Events.Publish(new NodeRedrawEvent(target));
        }
    }

    private void HandleOperatorChange(OperatorNodeEvents.OperatorChangedEvent ev)
    {
       ev.Node.Calculate();
    }

    private void HandleNumberChange(NumberNodeChangedEvent ev)
    {
        if (ev.Node.Port.Target is { } target)
        {
            target.Calculate();
            _diagramService.Events.Publish(new NodeRedrawEvent(target));
        }
    }
}