using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.NumberNode;

public partial class NumberNode : Node, IHasComponent<NumberNodeComponent>, INumberOutput
{
    private decimal? _numberOutput;

    public decimal? NumberOutput
    {
        get => _numberOutput;
        set
        {
            _numberOutput = value;
            NotifyNumberChanged();
        }
    }

    public void NotifyNumberChanged()
    {
        OnNumberChanged.Publish(new (NumberOutput));
    }

    public NumberNode()
    {
        var exitPort = new NumberNodeOutputPort
        {
            Alignment = PortAlignment.Right,
        };

        Ports.Add(exitPort);
    }
}