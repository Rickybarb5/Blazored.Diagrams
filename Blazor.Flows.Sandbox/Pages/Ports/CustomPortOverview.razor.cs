using Blazor.Flows.Links;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;
using Blazor.Flows.Services.Diagrams;
using Microsoft.AspNetCore.Components;

namespace Blazor.Flows.Sandbox.Pages.Ports;

public partial class CustomPortOverview
{
    [Inject] public IDiagramService Service { get; set; } = null!;

    protected override Task OnInitializedAsync()
    {
        // 1. Create a node with a DEFAULT port
        var node1 = new Node();
        node1.SetPosition(50, 150);
        Service.AddNode(node1);

        var defaultPort = new Port { Alignment = PortAlignment.Right, Justification = PortJustification.Center };
        Service.AddPortTo(node1, defaultPort);

        // 2. Create a node with a CUSTOM port
        var node2 = new Node();
        node2.SetPosition(400, 150);
        Service.AddNode(node2);

        // Use the new CustomPort model
        var customPort = new CustomPort { Alignment = PortAlignment.Left, Justification = PortJustification.Center };
        Service.AddPortTo(node2, customPort);

        // 3. Add a link between them
        Service.AddLinkTo(defaultPort, customPort, new LineLink());

        return base.OnInitializedAsync();
    }
}