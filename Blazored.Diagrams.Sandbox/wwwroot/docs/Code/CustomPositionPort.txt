using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Sandbox.Pages.Ports;

public class CustomPositionPort : Port
{
    public override (int PositionX, int PositionY) CustomPositioning()
    {
        // Position this port 25% down and 25% across the parent node
        var x = Parent.PositionX + (Parent.Width / 4);
        var y = Parent.PositionY + (Parent.Height / 4);
        return (x, y);
    }
}