using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.NumberNode;

public class NumberNode : Node, IHasComponent<NumberNodeComponent>, INumberOutput
{
    public event Action<decimal?> OnNumberChanged;

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
        OnNumberChanged?.Invoke(NumberOutput);
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