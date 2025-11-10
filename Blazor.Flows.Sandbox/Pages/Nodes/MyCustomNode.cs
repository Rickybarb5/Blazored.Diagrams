using Blazor.Flows.Nodes;
using Blazor.Flows.Services.Registry;

namespace Blazor.Flows.Sandbox.Pages.Nodes;

public class MyCustomNode : Node, IHasComponent<MyCustomComponent>
{
    
    public string Name ="Custom Logic Node";
    public string Status => Ports.Any() ? $"{Ports.Count} Ports Added!" : "No Ports";
}