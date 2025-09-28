using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.NumberNode;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.Ports;

public class NumberInputPort : Port
{
    public override bool CanCreateLink()
    {
        return false;
    }

    public override bool CanConnectTo(IPort port)
    {
        return port is NumberNodeOutputPort;
    }
}