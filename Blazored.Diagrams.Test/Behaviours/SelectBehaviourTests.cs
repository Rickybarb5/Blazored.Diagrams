using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Test.Behaviours;

public class SelectBehaviourTests
{
    private IDiagramService CreateService()
    {
        var service = new DiagramService();
        service.Create<Diagram>();

        return service;
    }

    [Fact]
    public void NodePointerDown_WhenLeftClick_ShouldSelectNode()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node();
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs { Button = 0 };

        // Act
        service.Events.Publish(new NodePointerDownEvent(node, args));

        // Assert
        Assert.True(node.IsSelected);
    }

    [Fact]
    public void NodePointerDown_WhenRightClick_ShouldNotSelectNode()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node();
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs { Button = 2 }; // Right click

        // Act
        service.Events.Publish(new NodePointerDownEvent(node, args));

        // Assert
        Assert.False(node.IsSelected);
    }

    [Fact]
    public void CtrlClick_WithMultiSelectEnabled_ShouldToggleSelection()
    {
        // Arrange
        using var service = CreateService();
        var node1 = new Node();
        var node2 = new Node();
        var layer = new Layer();
        layer.Nodes.Add(node1);
        layer.Nodes.Add(node2);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs { Button = 0, CtrlKey = true };

        // Act - Select both nodes
        service.Events.Publish(new NodePointerDownEvent(node1, args));
        service.Events.Publish(new NodePointerDownEvent(node2, args));

        // Assert
        Assert.True(node1.IsSelected);
        Assert.True(node2.IsSelected);

        // Act - Deselect first node
        service.Events.Publish(new NodePointerDownEvent(node1, args));

        // Assert
        Assert.False(node1.IsSelected);
        Assert.True(node2.IsSelected);
    }

    [Fact]
    public void CtrlClick_WithMultiSelectDisabled_ShouldSelectOnlyLastClicked()
    {
        // Arrange
        using var service = CreateService();
        service.Diagram.Options.Get<SelectOptions>().MultiSelectEnabled = false;

        var node1 = new Node();
        var node2 = new Node();
        var layer = new Layer();
        layer.Nodes.Add(node1);
        layer.Nodes.Add(node2);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs { Button = 0, CtrlKey = true };

        // Act
        service.Events.Publish(new NodePointerDownEvent(node1, args));
        service.Events.Publish(new NodePointerDownEvent(node2, args));

        // Assert
        Assert.False(node1.IsSelected);
        Assert.True(node2.IsSelected);
    }

    [Fact]
    public void BackgroundClick_ShouldDeselectAll()
    {
        // Arrange
        using var service = CreateService();
        var node1 = new Node { IsSelected = true };
        var node2 = new Node { IsSelected = true };
        var layer = new Layer();
        layer.Nodes.Add(node1);
        layer.Nodes.Add(node2);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs { Button = 0 };

        // Act
        service.Events.Publish(new DiagramPointerDownEvent(service.Diagram, args));

        // Assert
        Assert.False(node1.IsSelected);
        Assert.False(node2.IsSelected);
    }

    [Fact]
    public void BackgroundCtrlClick_ShouldNotDeselectAll()
    {
        // Arrange
        using var service = CreateService();
        var node1 = new Node { IsSelected = true };
        var node2 = new Node { IsSelected = true };
        var layer = new Layer();
        layer.Nodes.Add(node1);
        layer.Nodes.Add(node2);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs { Button = 0, CtrlKey = true };

        // Act
        service.Events.Publish(new DiagramPointerDownEvent(service.Diagram, args));

        // Assert
        Assert.True(node1.IsSelected);
        Assert.True(node2.IsSelected);
    }

    [Fact]
    public void WhenSelectionDisabled_ShouldNotSelectAnything()
    {
        // Arrange
        using var service = CreateService();
        service.Diagram.Options.Get<SelectOptions>().SelectionEnabled = false;

        var node = new Node();
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs { Button = 0 };

        // Act
        service.Events.Publish(new NodePointerDownEvent(node, args));

        // Assert
        Assert.False(node.IsSelected);
    }

    [Fact]
    public void SelectionShouldWorkForAllSelectableTypes()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node();
        var group = new Group();
        var sourcePort = new Port();
        var targetPort = new Port();
        var link = new Link() { SourcePort = sourcePort, TargetPort = targetPort };

        var layer = new Layer();
        layer.Nodes.Add(node);
        layer.Groups.Add(group);
        node.Ports.Add(sourcePort);
        group.Ports.Add(targetPort);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs { Button = 0 };

        // Act & Assert
        service.Events.Publish(new NodePointerDownEvent(node, args));
        Assert.True(node.IsSelected);

        service.Events.Publish(new GroupPointerDownEvent(group, args));
        Assert.True(group.IsSelected);
        Assert.False(node.IsSelected); // Previous selection should be cleared

        service.Events.Publish(new LinkPointerDownEvent(link, args));
        Assert.True(link.IsSelected);
        Assert.False(group.IsSelected); // Previous selection should be cleared
    }

    [Fact]
    public void IsEnabled_WhenSetToFalse_ShouldNotProcessEvents()
    {
        // Arrange
        using var service = CreateService();
        service.Diagram.Options.Get<SelectOptions>().IsEnabled = false;

        var node = new Node();
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs { Button = 0 };

        // Act
        service.Events.Publish(new NodePointerDownEvent(node, args));

        // Assert
        Assert.False(node.IsSelected);
    }

    [Fact]
    public void IsEnabled_WhenReEnabled_ShouldProcessEvents()
    {
        // Arrange
        using var service = CreateService();
        service.Diagram.Options.Get<SelectOptions>().IsEnabled = false;
        service.Diagram.Options.Get<SelectOptions>().IsEnabled = true;

        var node = new Node();
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs { Button = 0 };

        // Act
        service.Events.Publish(new NodePointerDownEvent(node, args));

        // Assert
        Assert.True(node.IsSelected);
    }

    [Fact]
    public void Dispose_ShouldStopProcessingEvents()
    {
        // Arrange
        using var service = CreateService();
        var node = new Node();
        var layer = new Layer();
        layer.Nodes.Add(node);
        service.Diagram.Layers.Add(layer);

        var args = new PointerEventArgs { Button = 0 };

        // Verify initial behavior
        service.Events.Publish(new NodePointerDownEvent(node, args));
        Assert.True(node.IsSelected);

        // Reset selection
        node.IsSelected = false;

        // Dispose and test
        service.Dispose();

        // Act
        service.Events.Publish(new NodePointerDownEvent(node, args));

        // Assert
        Assert.False(node.IsSelected);
    }
}