using Blazored.Diagrams.Services.Events;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample;

public interface INumberOutput
{
    public decimal? NumberOutput { get; set; }
    
    [JsonIgnore]
    ITypedEvent<NumberNode.NumberNode.NumberNodeChangedEvent> OnNumberChanged { get; init; }
    void NotifyNumberChanged();
}