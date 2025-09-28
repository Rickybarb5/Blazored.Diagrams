using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.OperatorNode;

public class OperatorInputPort : Port
{
    public new OperatorNode Parent { get; set; }

    public override bool CanConnectTo(IPort port)
    {
        return port.Parent is INumberOutput && IncomingLinks.Count < 2;
    }

    public override bool CanCreateLink()
    {
        return false;
    }

    private IEnumerable<decimal?> GetNumbers()
    {
        return IncomingLinks.Select(x => x.SourcePort.Parent).Cast<INumberOutput>().Select(x => x.NumberOutput);
    }
}