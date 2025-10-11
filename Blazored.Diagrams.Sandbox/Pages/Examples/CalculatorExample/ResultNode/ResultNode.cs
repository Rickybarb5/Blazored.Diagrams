using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.NumberNode;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.ResultNode;

public class ResultNode : Node, IHasComponent<ResultNodeComponent>, INumberOutput
{
    public ResultInputPort InputPort => (ResultInputPort)Ports.First(x => (x.GetType() == typeof(ResultInputPort)));
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
    public ITypedEvent<NumberNode.NumberNode.NumberNodeChangedEvent> OnNumberChanged { get; init; } = new TypedEvent<NumberNode.NumberNode.NumberNodeChangedEvent>();

    public void NotifyNumberChanged()
    {
        OnNumberChanged.Publish(new(NumberOutput));
    }

    public ResultNode()
    {
        var inputPort = new ResultInputPort
        {
            Alignment = PortAlignment.Left,
            Justification = PortJustification.Center,
        };

        var exitPort = new NumberNodeOutputPort
        {
            Alignment = PortAlignment.Right,
            Justification = PortJustification.Center,
        };

        Ports.Add(inputPort);
        Ports.Add(exitPort);
    }
}