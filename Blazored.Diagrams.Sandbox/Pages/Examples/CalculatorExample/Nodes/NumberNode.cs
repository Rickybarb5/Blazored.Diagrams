using Blazored.Diagrams.Events;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.Components;
using Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.Ports;
using Blazored.Diagrams.Services.Registry;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.Nodes;

public class NumberNode : Node, IHasComponent<NumberNodeComponent>, INumberResult
{
    private decimal? _number;
    public NumberNodePort Port { get; init; }
    
    [JsonIgnore]
    public ITypedEvent<NumberNodeChangedEvent> OnNumberChanged { get; init; } = new TypedEvent<NumberNodeChangedEvent>();
    public decimal? NumberResult
    {
        get => _number;
        set
        {
            _number = value;
            OnNumberChanged.Publish(new (this));
        }
    }
    
    public NumberNode()
    {
        Port = new NumberNodePort
        {
            Parent = this,
            Alignment = PortAlignment.Right,
        };
        AddPortInternal(Port);
    }

}
public record NumberNodeChangedEvent(NumberNode Node) : IEvent;