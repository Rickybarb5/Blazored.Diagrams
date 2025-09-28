using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.Organigram;

public class OrganigramNode : Node, IHasComponent<OrganigramNodeComponent>
{
    public string Job = "Unknown";
}