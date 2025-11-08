using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Diagrams;
using Microsoft.AspNetCore.Components;

namespace Blazored.Diagrams.Sandbox.Pages.Links;

public partial class CustomLinkOverview
{
    [Inject] public IDiagramService Service { get; set; } = null!;

    protected override Task OnInitializedAsync()
    {
        // 1. Create Nodes
        var n1 = new Node { PositionX = 50, PositionY = 100 };
        var n2 = new Node { PositionX = 300, PositionY = 50 };
        var n3 = new Node { PositionX = 301, PositionY = 300 };

        // Set size for all nodes (for better visual appeal)
        n1.SetSize(140, 50);
        n2.SetSize(140, 50);
        n3.SetSize(140, 50);

        // 2. Add Nodes
        Service.AddNode(n1);
        Service.AddNode(n2);
        Service.AddNode(n3);

        // 3. Create Ports for Links

        // Ports for the Extended Link (n1 -> n3)
        Service.AddPortTo(n1, new Port { Alignment = PortAlignment.Right });
        Service.AddPortTo(n3, new Port { Alignment = PortAlignment.Left });

        // Ports for the Animated Link (n2 -> n3)
        Service.AddPortTo(n2, new Port { Alignment = PortAlignment.Bottom });
        Service.AddPortTo(n3, new Port { Alignment = PortAlignment.Top });

        var extendedLink = new ExtendedLink();
        var animatedLink = new AnimatedLink();


        // 5. Add Links by connecting specific ports

        // Link 1: ExtendedLink (n1 Right -> n3 Left)
        var n1RightPort = n1.Ports.First(p => p.Alignment == PortAlignment.Right);
        var n3LeftPort = n3.Ports.First(p => p.Alignment == PortAlignment.Left);
        Service.AddLinkTo(n1RightPort, n3LeftPort, extendedLink);

        // Link 2: AnimatedLink (n2 Bottom -> n3 Top)
        var n2BottomPort = n2.Ports.First(p => p.Alignment == PortAlignment.Bottom);
        var n3TopPort = n3.Ports.First(p => p.Alignment == PortAlignment.Top);
        Service.AddLinkTo(n2BottomPort, n3TopPort, animatedLink);

        return base.OnInitializedAsync();
    }
}