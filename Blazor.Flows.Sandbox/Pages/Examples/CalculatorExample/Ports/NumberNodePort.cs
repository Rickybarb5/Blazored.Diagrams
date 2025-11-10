using Blazor.Flows.Ports;
using Blazor.Flows.Sandbox.Pages.Examples.CalculatorExample.Nodes;

namespace Blazor.Flows.Sandbox.Pages.Examples.CalculatorExample.Ports;

public class NumberNodePort : Port
{
    public new required NumberNode Parent { get; set; }
    public Nodes.OperatorNode? Target => OutgoingLinks.FirstOrDefault()?.TargetPort?.Parent as Nodes.OperatorNode;

    public override bool CanCreateLink()
    {
        return OutgoingLinks.Count == 0;
    }

    public override bool CanConnectTo(IPort targetPort)
    {
        return targetPort is OperatorInputPort;
    }
}