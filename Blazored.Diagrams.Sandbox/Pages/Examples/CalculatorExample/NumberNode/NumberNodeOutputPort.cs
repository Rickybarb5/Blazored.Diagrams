using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.NumberNode;

public class NumberNodeOutputPort : Port
{
    public override bool CanCreateLink()
    {
        return OutgoingLinks.Count < 1;
    }
}