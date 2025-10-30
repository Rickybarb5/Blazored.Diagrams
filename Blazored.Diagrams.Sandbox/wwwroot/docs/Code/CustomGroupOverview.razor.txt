using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Services.Diagrams;
using Microsoft.AspNetCore.Components;

namespace Blazored.Diagrams.Sandbox.Pages.Groups;

public partial class CustomGroupOverview
{
    [Inject] public IDiagramService Service { get; set; } = null!;

    protected override Task OnInitializedAsync()
    {
        // The CustomGroup class must be defined/available in the project for this to compile.
        var customGroup = new CustomGroup
        {
            Title = "Department A - Infrastructure",
            Width = 400,
            Height = 300
        };
        customGroup.SetPosition(50, 50);

        Service.AddGroup(customGroup);

        var node1 = new Node();
        node1.SetPosition(50, 80);
        node1.SetSize(100, 50);

        Service.AddNodeTo(customGroup, node1);

        var node2 = new Node();
        node2.SetPosition(250, 150);
        node2.SetSize(100, 50);
        Service.AddNodeTo(customGroup, node2);

        var outsideNode = new Node();
        outsideNode.SetPosition(500, 150);
        Service.AddNode(outsideNode);

        return base.OnInitializedAsync();
    }
}