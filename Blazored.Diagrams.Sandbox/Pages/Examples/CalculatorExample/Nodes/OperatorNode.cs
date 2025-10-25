using Blazored.Diagrams.Events;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.Components;
using Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.OperatorNode;
using Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.Ports;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.Nodes;

public class OperatorNode : Node, IHasComponent<OperatorNodeComponent>, INumberResult
{
    public OperatorInputPort InputPort { get; init; }
    public OperatorOutputPort OutputPort { get; init; }

    private Operator _operator;

    public Operator Operator
    {
        get => _operator;
        set
        {
            _operator = value;
            OnOperatorChanged.Publish(new OperatorChangedEvent(this));
        }
    }

    public OperatorNode()
    {
        InputPort = new OperatorInputPort
        {
            Parent = this,
            Alignment = PortAlignment.Left,
        };
        Ports.Add(InputPort);

        OutputPort = new OperatorOutputPort
        {
            Parent = this,
            Alignment = PortAlignment.Right,
        };
        Ports.Add(OutputPort);
    }
    
  
    public ITypedEvent<OperationResultChangedEvent> OnOperationResultChanged { get; init; } =
        new TypedEvent<OperationResultChangedEvent>();

    public ITypedEvent<OperatorChangedEvent> OnOperatorChanged { get; init; } =
        new TypedEvent<OperatorChangedEvent>();

    public record OperatorChangedEvent(OperatorNode Node) : IEvent;
    public record OperationResultChangedEvent(OperatorNode Node) : IEvent;

    public void Calculate()
    {
        var numbers = GetNumbers();
        switch (_operator)
        {
            case Operator.Add:
                NumberResult = numbers.Sum();
                break;
            case Operator.Multiply:
                NumberResult = numbers.Aggregate((i, j) => i * j);
                break;
            case Operator.Subtract:
                NumberResult = numbers.Aggregate((i, j) => i - j);
                break;
            case Operator.Divide:
                try
                {
                    NumberResult = numbers.Aggregate((i, j) => i /j);
                }
                catch (Exception e)
                {
                    NumberResult = null;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public IEnumerable<decimal?> GetNumbers()
    {
       return InputPort.IncomingLinks
            .Select(l => l.SourcePort.Parent)
            .OfType<INumberResult>()
            .Select(p => p.NumberResult)
            .Where(n=> n is not null);
    }

    private decimal? _operationResult;
    public decimal? NumberResult
    {
        get => _operationResult;
        set
        {
            _operationResult = value;
            OnOperationResultChanged.Publish(new(this));
        }
    }
}