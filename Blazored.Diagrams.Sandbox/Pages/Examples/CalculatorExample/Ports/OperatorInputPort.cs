using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.Ports;

public class OperatorInputPort : Port
{
    
    public new required Nodes.OperatorNode Parent { get; set; }

    public override bool CanCreateLink()
    {
        return false;
    }
}