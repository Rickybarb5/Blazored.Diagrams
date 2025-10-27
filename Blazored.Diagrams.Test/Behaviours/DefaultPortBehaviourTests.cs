using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Diagrams;


namespace Blazored.Diagrams.Test.Behaviours;

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
        service.Add.Node(node);

        // Act
        service.Add.PortTo(node, port);

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
        service.Add.Group(group);

        // Act
        service.Add.PortTo(group, port);

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
        service.Add.PortTo(sourceNode, sourcePort);
        service.Add.PortTo(targetNode, targetPort);
        service.Add.Node(sourceNode);
        service.Add.Node(targetNode);
        var link = new LineLink();

        // Act
        service.Add.AddLinkTo(sourcePort, targetPort, link);

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
        service.Add.PortTo(sourceNode, sourcePort);
        service.Add.PortTo(targetNode, targetPort);
        service.Add.Node(sourceNode);
        service.Add.Node(targetNode);

        var link = new LineLink();
        service.Add.AddLinkTo<Link>(sourcePort, targetPort, link);

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
        service.Add.PortTo(sourceNode, sourcePort);
        service.Add.PortTo(targetNode, targetPort);
        service.Add.Node(sourceNode);
        service.Add.Node(targetNode);

        var link = new LineLink();
        service.Add.AddLinkTo(sourcePort, targetPort, link);

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
        service.Add.Node(node);
        service.Add.PortTo(node, port);

        var initialX = port.PositionX;
        var initialY = port.PositionY;

        // Act
        node.SetPosition(50, 50);

        // Assert
        Assert.Equal(initialX, port.PositionX);
        Assert.Equal(initialY, port.PositionY);
    }
}