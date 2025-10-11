using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.OperatorNode;

public class OperatorNode : Node, IHasComponent<OperatorNodeComponent>, INumberOutput
{
    public OperatorInputPort? InputPort =>
        (OperatorInputPort?)Ports.FirstOrDefault(x => x.GetType() == typeof(OperatorInputPort));

    public OperatorOutputPort? OutputPort =>
        (OperatorOutputPort?)Ports.FirstOrDefault(x => x.GetType() == typeof(OperatorOutputPort));

    private Operator _operator;

    public Operator Operator
    {
        get => _operator;
        set
        {
            _operator = value;
            NotifyOperatorChanged();
        }
    }

    public event Action<string>? OnError;

    public event Action<Operator>? OnOperatorChanged;

    public OperatorNode()
    {
        if (InputPort is null)
        {
            var inputPort = new OperatorInputPort
            {
                Alignment = PortAlignment.Left,
            };
            Ports.Add(inputPort);
        }

        if (OutputPort is null)
        {
            var outputPort = new OperatorOutputPort
            {
                Alignment = PortAlignment.Right,
            };
            Ports.Add(outputPort);
        }
    }

    public void NotifyError(string error)
    {
        OnError?.Invoke(error);
    }

    public void NotifyOperatorChanged()
    {
        OnOperatorChanged.Invoke(Operator);
    }

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
}