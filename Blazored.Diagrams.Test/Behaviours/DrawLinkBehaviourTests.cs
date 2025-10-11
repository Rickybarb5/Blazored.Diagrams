using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Diagrams;
using Blazored.Diagrams.Services.Events;

using Microsoft.AspNetCore.Components.Web;
using Moq;

namespace Blazored.Diagrams.Test.Behaviours;

public class DrawLinkBehaviorTests
{
    private IDiagramService CreateService()
    {
        var service = new DiagramService();
        return service;
    }

    [Fact]
    public void WhenPortClicked_ShouldStartLinkCreation()
    {
        // Arrange
        using var service = CreateService();
        var sourceNode = new Node();
        var sourcePort = new Port();
        sourceNode.Ports.Add(sourcePort);
        service.Add.Node(sourceNode);

        DrawLinkStartEvent? capturedEvent = null;
        service.Events.SubscribeTo<DrawLinkStartEvent>(e => capturedEvent = e);

        var args = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100,
            Buttons = 1
        };

        // Act
        service.Events.Publish(new PortPointerDownEvent(sourcePort, args));

        // Assert
        Assert.NotNull(capturedEvent);
        Assert.NotNull(capturedEvent.Link);
        Assert.Equal(sourcePort, capturedEvent.Link.SourcePort);
        Assert.Null(capturedEvent.Link.TargetPort);
    }

    [Fact]
    public void WhenMouseMoves_ShouldUpdateLinkPosition()
    {
        // Arrange
        using var service = CreateService();
        var sourceNode = new Node();
        var sourcePort = new Port();
        sourceNode.Ports.Add(sourcePort);
        service.Add.Node(sourceNode);

        // Start link creation
        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100,
            Buttons = 1
        };
        service.Events.Publish(new PortPointerDownEvent(sourcePort, startArgs));

        // Get the created link
        var link = service.Diagram.AllLinks.First();
        var initialX = link.TargetPositionX;
        var initialY = link.TargetPositionY;

        // Act
        var moveArgs = new PointerEventArgs
        {
            ClientX = 200,
            ClientY = 200,
            Buttons = 1
        };
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, moveArgs));

        // Assert
        Assert.NotEqual(initialX, link.TargetPositionX);
        Assert.NotEqual(initialY, link.TargetPositionY);
    }

    [Fact]
    public void WhenReleasedOnValidPort_ShouldCreateLink()
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

        DrawLinkCreatedEvent? capturedEvent = null;
        service.Events.SubscribeTo<DrawLinkCreatedEvent>(e => capturedEvent = e);

        // Start link creation
        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100,
            Buttons = 1
        };
        service.Events.Publish(new PortPointerDownEvent(sourcePort, startArgs));

        // Act
        service.Events.Publish(new PortPointerUpEvent(targetPort, new PointerEventArgs()));

        // Assert
        Assert.NotNull(capturedEvent);
        Assert.Equal(sourcePort, capturedEvent.Link.SourcePort);
        Assert.Equal(targetPort, capturedEvent.Link.TargetPort);
        Assert.Contains(capturedEvent.Link, targetPort.IncomingLinks);
    }

    [Fact]
    public void WhenReleasedOnInvalidPort_ShouldCancelLinkCreation()
    {
        // Arrange
        using var service = CreateService();
        var sourceNode = new Node();
        var sourcePort = new Mock<IPort>();
        sourcePort.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());
        sourcePort.Setup(x => x.CanCreateLink()).Returns(false);
        sourcePort.Setup(x => x.Parent).Returns(sourceNode);
        sourcePort.Setup(x => x.IncomingLinks).Returns([]);
        sourcePort.Setup(x => x.OutgoingLinks).Returns([]);
        sourceNode.Ports.Add(sourcePort.Object);
        service.Add.Node(sourceNode);

        DrawLinkCancelledEvent? capturedEvent = null;
        service.Events.SubscribeTo<DrawLinkCancelledEvent>(e => capturedEvent = e);

        // Start link creation
        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100,
            Buttons = 1
        };
        service.Events.Publish(new PortPointerDownEvent(sourcePort.Object, startArgs));

        // Act
        service.Events.Publish(new DiagramPointerUpEvent(service.Diagram, new PointerEventArgs()));

        // Assert
        Assert.NotNull(capturedEvent);
    }

    [Fact]
    public void WhenLinkCancelled_ShouldCleanupUnboundedLinks()
    {
        // Arrange
        using var service = CreateService();
        var sourceNode = new Node();
        var sourcePort = new Port();
        sourceNode.Ports.Add(sourcePort);
        service.Add.Node(sourceNode);

        // Start link creation
        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100,
            Buttons = 1
        };
        service.Events.Publish(new PortPointerDownEvent(sourcePort, startArgs));

        // Act
        service.Events.Publish(new DiagramPointerUpEvent(service.Diagram, new PointerEventArgs()));

        // Assert
        Assert.Empty(service.Diagram.AllLinks.Where(l => l.TargetPort == null));
    }

    [Fact]
    public void IsEnabled_WhenSetToFalse_ShouldNotCreateLinks()
    {
        // Arrange
        using var service = CreateService();
        service.Behaviours.GetBehaviourOptions<DrawLinkBehaviourOptions>().IsEnabled = false;

        var sourceNode = new Node();
        var sourcePort = new Port();
        sourceNode.Ports.Add(sourcePort);
        service.Add.Node(sourceNode);

        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100,
            Buttons = 1
        };

        // Act
        service.Events.Publish(new PortPointerDownEvent(sourcePort, startArgs));

        // Assert
        Assert.Empty(service.Diagram.AllLinks);
    }
}