using Blazor.Flows.Nodes;
using Blazor.Flows.Sandbox.Components;
using Blazor.Flows.Services.Registry;

namespace Blazor.Flows.Sandbox.Models;

public class NamedNode : Node, IHasComponent<NamedNodeComponent>
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } ="gray";
    public string Message { get; set; } = string.Empty;
}