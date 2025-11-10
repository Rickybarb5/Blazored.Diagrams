using Blazor.Flows.Events;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;
using Blazor.Flows.Sandbox.Pages.Examples.CalculatorExample.Components;
using Blazor.Flows.Sandbox.Pages.Examples.CalculatorExample.Ports;
using Blazor.Flows.Services.Registry;
using Newtonsoft.Json;

namespace Blazor.Flows.Sandbox.Pages.Examples.CalculatorExample.Nodes;

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