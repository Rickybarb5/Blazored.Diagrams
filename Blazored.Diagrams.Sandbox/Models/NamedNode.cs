using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Sandbox.Components;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Sandbox.Models;

public class NamedNode : Node, IHasComponent<NamedNodeComponent>
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } ="gray";
    public string Message { get; set; } = string.Empty;
}