using Blazor.Flows.Groups;
using Blazor.Flows.Links;
using Blazor.Flows.Nodes;
using Blazor.Flows.Options.Behaviours;
using Blazor.Flows.Ports;
using Blazor.Flows.Services.Diagrams;


namespace Blazor.Flows.Test.Behaviours;

public class DefaultPortBehaviourTests
{
    private IDiagramService CreateService()
    {
        var service = new DiagramService();
        return service;
    }

    [Fact]
    public void WhenPortAddedToNode_ShouldSetNodeAsParent()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node();
        var port = new Port();
        service.AddNode(node);

        // Act
        service.AddPortTo(node, port);

        // Assert
        Assert.Equal(node, port.Parent);
    }

    [Fact]
    public void WhenPortAddedToGroup_ShouldSetGroupAsParent()
    {
        // Arrange
        using var service = CreateService();
        var group = new Group();
        var port = new Port();
        service.AddGroup(group);

        // Act
        service.AddPortTo(group, port);

        // Assert
        Assert.Equal(group, port.Parent);
    }

    [Fact]
    public void WhenOutgoingLinkAdded_ShouldSetSourcePort()
    {
        // Arrange
        using var service = CreateService();
        var sourceNode = new Node();
        var targetNode = new Node();
        var sourcePort = new Port();
        var targetPort = new Port();
        service.AddPortTo(sourceNode, sourcePort);
        service.AddPortTo(targetNode, targetPort);
        service.AddNode(sourceNode);
        service.AddNode(targetNode);
        var link = new LineLink();

        // Act
        service.AddLinkTo(sourcePort, targetPort, link);

        // Assert
        Assert.Equal(sourcePort, link.SourcePort);
    }


    [Fact]
    public void WhenIncomingLinkRemoved_ShouldClearTargetPort()
    {
        // Arrange
        using var service = CreateService();
        var sourceNode = new Node();
        var targetNode = new Node();
        var sourcePort = new Port();
        var targetPort = new Port();
        service.AddPortTo(sourceNode, sourcePort);
        service.AddPortTo(targetNode, targetPort);
        service.AddNode(sourceNode);
        service.AddNode(targetNode);

        var link = new LineLink();
        service.AddLinkTo(sourcePort, targetPort, link);

        // Act
        targetPort.IncomingLinks.RemoveInternal(link);

        // Assert
        Assert.Null(link.TargetPort);
    }

    [Fact]
    public void WhenOutgoingLinkRemoved_ShouldDisposeLinkWithoutSourcePort()
    {
        // Arrange
        using var service = CreateService();
        var sourceNode = new Node();
        var targetNode = new Node();
        var sourcePort = new Port();
        var targetPort = new Port();
        service.AddPortTo(sourceNode, sourcePort);
        service.AddPortTo(targetNode, targetPort);
        service.AddNode(sourceNode);
        service.AddNode(targetNode);

        var link = new LineLink();
        service.AddLinkTo(sourcePort, targetPort, link);

        // Act
        sourcePort.OutgoingLinks.RemoveInternal(link);

        // Assert
        Assert.DoesNotContain(link, service.Diagram.AllLinks);
    }

    [Fact]
    public void IsEnabled_WhenSetToFalse_ShouldNotProcessEvents()
    {
        // Arrange
        using var service = CreateService();
        service.Behaviours.GetBehaviourOptions<DefaultPortBehaviourOptions>()!.IsEnabled = false;

        var node = new Node { Width = 100, Height = 100 };
        var port = new Port { Width = 10, Height = 10 };
        service.AddNode(node);
        service.AddPortTo(node, port);

        var initialX = port.PositionX;
        var initialY = port.PositionY;

        // Act
        node.SetPosition(50, 50);

        // Assert
        Assert.Equal(initialX, port.PositionX);
        Assert.Equal(initialY, port.PositionY);
    }
}