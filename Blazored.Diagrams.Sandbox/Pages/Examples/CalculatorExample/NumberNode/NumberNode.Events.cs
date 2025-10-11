using Blazored.Diagrams.Services.Events;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.NumberNode;

public partial class NumberNode
{
    public record NumberNodeChangedEvent(decimal? value) : IEvent;

    [JsonIgnore]
    public ITypedEvent<NumberNodeChangedEvent> OnNumberChanged { get; init; } = new TypedEvent<NumberNodeChangedEvent>();

}