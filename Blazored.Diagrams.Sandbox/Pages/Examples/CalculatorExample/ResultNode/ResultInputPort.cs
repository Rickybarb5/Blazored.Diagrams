using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.OperatorNode;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.ResultNode;

public class ResultInputPort : Port
{
    public override bool CanCreateLink()
    {
        return false;
    }

    public override bool CanConnectTo(IPort port)
    {
        if (port.IncomingLinks.Count == 0 && port is OperatorInputPort)
        {
            return true;
        }

        return base.CanConnectTo(port);
    }
}