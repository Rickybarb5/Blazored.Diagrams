using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Sandbox.Pages.Nodes;

public class MyCustomNode : Node, IHasComponent<MyCustomComponent>
{
    
    public string Name ="Custom Logic Node";
    public string Status => Ports.Any() ? $"{Ports.Count} Ports Added!" : "No Ports";
}