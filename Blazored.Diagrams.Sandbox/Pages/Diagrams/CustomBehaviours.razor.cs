using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Diagrams;
using Microsoft.AspNetCore.Components;

namespace Blazored.Diagrams.Sandbox.Pages.Diagrams;

public partial class CustomBehaviours
{
    [Inject] public IDiagramService Service { get; set; } = null!;
    private readonly DoubleTapSelectOptions _options = new();

    protected override Task OnInitializedAsync()
    {
        // Register the custom behaviour, associating it with the options
        Service.Behaviours.RegisterBehaviourOptions(_options);
        Service.Behaviours.RegisterBehaviour(new DoubleTapSelectBehaviour(Service));

        // Add demo nodes and links
        var node1 = new Node();
        node1.SetPosition(100, 100);
        Service.AddNode(node1);
        Service.AddPortTo(node1, new Port { Alignment = PortAlignment.Right });

        var node2 = new Node();
        node2.SetPosition(400, 300);
        Service.AddNode(node2);
        Service.AddPortTo(node2, new Port { Alignment = PortAlignment.Left });

        Service.AddLinkTo(node1.Ports.First(), node2.Ports.First(), new LineLink());

        return base.OnInitializedAsync();
    }

}