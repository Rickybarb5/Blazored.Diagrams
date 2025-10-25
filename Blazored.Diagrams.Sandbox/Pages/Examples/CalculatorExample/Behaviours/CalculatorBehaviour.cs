using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.Nodes;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.Behaviours;

public class CalculatorBehaviour : BaseBehaviour
{
    private IDiagramService _diagramService;
    public CalculatorBehaviour(IDiagramService diagramService)
    {
        _diagramService = diagramService;
        Subscriptions =
        [
            _diagramService.Events.SubscribeTo<NumberNodeChangedEvent>(HandleNumberChange),
            _diagramService.Events.SubscribeTo<Nodes.OperatorNode.OperatorChangedEvent>(HandleOperatorChange),
            _diagramService.Events.SubscribeTo<Nodes.OperatorNode.OperationResultChangedEvent>(HandleResultChange),
        ];
    }

    private void HandleResultChange(Nodes.OperatorNode.OperationResultChangedEvent ev)
    {
        ev.Node.OutputPort.Target?.Calculate();
        if (ev.Node.OutputPort.Target is not null)
        {
            _diagramService.Events.Publish(new NodeRedrawEvent(ev.Node.OutputPort.Target));
        }
    }

    private void HandleOperatorChange(Nodes.OperatorNode.OperatorChangedEvent ev)
    {
       ev.Node.Calculate();
    }

    private void HandleNumberChange(NumberNodeChangedEvent ev)
    {
        ev.Node.Port.Target?.Calculate();
        if (ev.Node.Port.Target is not null)
        {
            _diagramService.Events.Publish(new NodeRedrawEvent(ev.Node.Port.Target));
        }
    }
    public new void Dispose()
    {
        Subscriptions.DisposeAll();
    }
}