using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.Ports;

public class OperatorOutputPort : Port
{
    public new required Nodes.OperatorNode Parent { get; set; }
    public Nodes.OperatorNode? Target => OutgoingLinks.FirstOrDefault()?.TargetPort?.Parent as Nodes.OperatorNode;

    public override bool CanConnectTo(IPort targetPort)
    {
        return targetPort is OperatorInputPort;
    }
}