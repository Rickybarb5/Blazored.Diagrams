using Blazored.Diagrams.Diagrams;
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
        node.Ports.Add(port);

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
        group.Ports.Add(port);

        // Assert
        Assert.Equal(group, port.Parent);
    }

    [Theory]
    [InlineData(PortAlignment.Left, PortJustification.Start)]
    [InlineData(PortAlignment.Left, PortJustification.End)]
    [InlineData(PortAlignment.Right, PortJustification.End)]
    [InlineData(PortAlignment.Right, PortJustification.Center)]
    [InlineData(PortAlignment.Right, PortJustification.Start)]
    [InlineData(PortAlignment.Top, PortJustification.Center)]
    [InlineData(PortAlignment.Top, PortJustification.Start)]
    [InlineData(PortAlignment.Top, PortJustification.End)]
    [InlineData(PortAlignment.Bottom, PortJustification.Start)]
    [InlineData(PortAlignment.Bottom, PortJustification.Center)]
    [InlineData(PortAlignment.Bottom, PortJustification.End)]
    [InlineData(PortAlignment.CenterParent, PortJustification.Center)]
    public void WhenPortAlignmentChanges_ShouldUpdatePosition(PortAlignment alignment, PortJustification justification)
    {
        // Arrange
        using var service = CreateService();
        var node = new Node { Width = 100, Height = 100, PositionX = 50, PositionY = 50 };
        var port = new Port { Width = 100, Height = 100 };
        service.Add.Node(node);
        node.Ports.Add(port);

        var initialX = port.PositionX;
        var initialY = port.PositionY;

        // Act
        port.Alignment = alignment;
        port.Justification = justification;

        // Assert
        Assert.True(port.PositionX != initialX || port.PositionY != initialY);
    }

    [Fact]
    public void WhenParentNodeMoves_ShouldUpdatePortPosition()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node { Width = 100, Height = 100, PositionX = 0, PositionY = 0 };
        var port = new Port { Width = 10, Height = 10, Alignment = PortAlignment.Left };
        service.Add.Node(node);
        node.Ports.Add(port);

        var initialX = port.PositionX;
        var initialY = port.PositionY;

        // Act
        node.SetPosition(50, 50);

        // Assert
        Assert.NotEqual(initialX, port.PositionX);
        Assert.NotEqual(initialY, port.PositionY);
    }

    [Fact]
    public void WhenIncomingLinkAdded_ShouldSetTargetPort()
    {
        // Arrange
        using var service = CreateService();
        var sourceNode = new Node();
        var targetNode = new Node();
        var sourcePort = new Port();
        var targetPort = new Port();
        sourceNode.Ports.Add(sourcePort);
        targetNode.Ports.Add(targetPort);
        service.Add.Node(sourceNode);
        service.Add.Node(targetNode);
        var link = new Link();

        // Act
        service.Add.AddLinkTo(sourcePort, targetPort, link);

        // Assert
        Assert.Equal(targetPort, link.TargetPort);
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
        sourceNode.Ports.Add(sourcePort);
        targetNode.Ports.Add(targetPort);
        service.Add.Node(sourceNode);
        service.Add.Node(targetNode);
        var link = new Link();

        // Act
        service.Add.AddLinkTo(sourcePort, targetPort, link);

        // Assert
        Assert.Equal(sourcePort, link.SourcePort);
    }

    [Fact]
    public void WhenParentNodeResizes_ShouldUpdatePortPosition()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node { Width = 100, Height = 100 };
        var port = new Port { Width = 10, Height = 10, Alignment = PortAlignment.Right };
        service.Add.Node(node);
        node.Ports.Add(port);

        var initialX = port.PositionX;

        // Act
        node.Width = 200;

        // Assert
        Assert.NotEqual(initialX, port.PositionX);
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
        sourceNode.Ports.Add(sourcePort);
        targetNode.Ports.Add(targetPort);
        service.Add.Node(sourceNode);
        service.Add.Node(targetNode);
        
        var link = new Link();
       service.Add.AddLinkTo<Link>(sourcePort, targetPort, link);

        // Act
        targetPort.IncomingLinks.Remove(link);

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
        sourceNode.Ports.Add(sourcePort);
        targetNode.Ports.Add(targetPort);
        service.Add.Node(sourceNode);
        service.Add.Node(targetNode);

        var link = new Link();
        service.Add.AddLinkTo(sourcePort, targetPort, link);

        // Act
        sourcePort.OutgoingLinks.Remove(link);

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
        node.Ports.Add(port);

        var initialX = port.PositionX;
        var initialY = port.PositionY;

        // Act
        node.SetPosition(50, 50);

        // Assert
        Assert.Equal(initialX, port.PositionX);
        Assert.Equal(initialY, port.PositionY);
    }
}