using Blazored.Diagrams.Events;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Test.Behaviours;

public class MoveBehaviourTests
{
    private IDiagramService CreateService()
    {
        var service = new DiagramService();
        return service;
    }

    [Fact]
    public void OnNodePointerDown_ShouldInitiateDragging()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node { IsSelected = true };
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100
        };

        // Act
        service.Events.Publish(new NodePointerDownEvent(node, args));

        // Assert
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, args));
        Assert.Equal(0, node.PositionX); // Position shouldn't change until mouse moves
    }

    [Fact]
    public void OnGroupPointerDown_ShouldInitiateDragging()
    {
        // Arrange
        using var service = CreateService();
        var group = new Group { IsSelected = true };
        var layer = new Layer();
        layer.Groups.Add(group);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100
        };

        // Act
        service.Events.Publish(new GroupPointerDownEvent(group, args));

        // Assert
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, args));
        Assert.Equal(0, group.PositionX); // Position shouldn't change until mouse moves
    }

    [Fact]
    public void OnPointerMove_WhenDragging_ShouldUpdateSelectedNodePosition()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node { IsSelected = true };
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100
        };
        var moveArgs = new PointerEventArgs
        {
            ClientX = 150,
            ClientY = 150
        };

        // Start dragging
        service.Events.Publish(new NodePointerDownEvent(node, startArgs));

        // Act
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, moveArgs));

        // Assert
        Assert.Equal(50, node.PositionX); // Moved 50px right
        Assert.Equal(50, node.PositionY); // Moved 50px down
    }

    [Fact]
    public void OnPointerMove_WhenDragging_ShouldUpdateSelectedGroupPosition()
    {
        // Arrange
        using var service = CreateService();
        var group = new Group { IsSelected = true };
        var layer = new Layer();
        layer.Groups.Add(group);
        service.Diagram.Layers.Add(layer);

        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100
        };
        var moveArgs = new PointerEventArgs
        {
            ClientX = 150,
            ClientY = 150
        };

        // Start dragging
        service.Events.Publish(new GroupPointerDownEvent(group, startArgs));

        // Act
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, moveArgs));

        // Assert
        Assert.Equal(50, group.PositionX); // Moved 50px right
        Assert.Equal(50, group.PositionY); // Moved 50px down
    }

    [Fact]
    public void OnPointerMove_WithZoom_ShouldAdjustMovementAccordingly()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node { IsSelected = true };
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);
        service.Diagram.SetZoom(2.0m); // Set zoom to 200%

        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100
        };
        var moveArgs = new PointerEventArgs
        {
            ClientX = 150,
            ClientY = 150
        };

        // Start dragging
        service.Events.Publish(new NodePointerDownEvent(node, startArgs));

        // Act
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, moveArgs));

        // Assert
        Assert.Equal(25, node.PositionX); // 50/2 due to zoom
        Assert.Equal(25, node.PositionY); // 50/2 due to zoom
    }

    [Fact]
    public void OnPointerUp_ShouldStopDragging()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node { IsSelected = true };
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100
        };
        var moveArgs = new PointerEventArgs
        {
            ClientX = 150,
            ClientY = 150
        };

        // Start dragging and move
        service.Events.Publish(new NodePointerDownEvent(node, startArgs));
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, moveArgs));

        // Act
        service.Events.Publish(new DiagramPointerUpEvent(service.Diagram, new PointerEventArgs()));
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, new PointerEventArgs
        {
            ClientX = 200,
            ClientY = 200
        }));

        // Assert
        Assert.Equal(50, node.PositionX); // Position should remain at last dragged position
        Assert.Equal(50, node.PositionY);
    }

    [Fact]
    public void Dispose_ShouldStopProcessingEvents()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node { IsSelected = true };
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100
        };
        var moveArgs = new PointerEventArgs
        {
            ClientX = 150,
            ClientY = 150
        };

        // Verify initial behavior
        service.Events.Publish(new NodePointerDownEvent(node, startArgs));
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, moveArgs));
        Assert.Equal(50, node.PositionX);

        // Reset position
        node.SetPosition(0, 0);

        // Dispose and test
        service.Dispose();

        // Act
        service.Events.Publish(new NodePointerDownEvent(node, startArgs));
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, moveArgs));

        // Assert
        Assert.Equal(0, node.PositionX); // Position should not change
        Assert.Equal(0, node.PositionY);
    }

    [Fact]
    public void IsEnabled_WhenSetToFalse_ShouldNotProcessEvents()
    {
        // Arrange
        using var service = CreateService();
        service.Behaviours.GetBehaviourOptions<MoveBehaviourOptions>().IsEnabled = false;

        var node = new Node { IsSelected = true };
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100
        };
        var moveArgs = new PointerEventArgs
        {
            ClientX = 150,
            ClientY = 150
        };

        // Act
        service.Events.Publish(new NodePointerDownEvent(node, startArgs));
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, moveArgs));

        // Assert
        Assert.Equal(0, node.PositionX); // Position should not change when disabled
        Assert.Equal(0, node.PositionY);
    }

    [Fact]
    public void IsEnabled_WhenReEnabled_ShouldProcessEvents()
    {
        // Arrange
        using var service = CreateService();
        service.Behaviours.GetBehaviourOptions<MoveBehaviourOptions>().IsEnabled = false;
        service.Behaviours.GetBehaviourOptions<MoveBehaviourOptions>().IsEnabled = true;

        var node = new Node { IsSelected = true };
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var startArgs = new PointerEventArgs
        {
            ClientX = 100,
            ClientY = 100
        };
        var moveArgs = new PointerEventArgs
        {
            ClientX = 150,
            ClientY = 150
        };

        // Act
        service.Events.Publish(new NodePointerDownEvent(node, startArgs));
        service.Events.Publish(new DiagramPointerMoveEvent(service.Diagram, moveArgs));

        // Assert
        Assert.Equal(50, node.PositionX); // Position should change when re-enabled
        Assert.Equal(50, node.PositionY);
    }
}