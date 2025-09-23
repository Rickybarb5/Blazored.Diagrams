using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Test.Behaviours;

public class DeleteWithKeyBehaviourTests
{
    private IDiagramService CreateService()
    {
        var service = new DiagramService();
        service.Create<Diagram>();

        return service;
    }

    [Fact]
    public void WhenDeletePressed_ShouldRemoveSelectedNodes()
    {
        // Arrange
        using var service = CreateService();

        var node1 = new Node { IsSelected = true };
        var node2 = new Node { IsSelected = false };
        service.AddNode(node1);
        service.AddNode(node2);

        var args = new KeyboardEventArgs { Code = "Delete" };

        // Act
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));

        // Assert
        Assert.DoesNotContain(node1, service.Diagram.AllNodes);
        Assert.Contains(node2, service.Diagram.AllNodes);
    }

    [Fact]
    public void WhenDeletePressed_ShouldRemoveSelectedGroups()
    {
        // Arrange
        using var service = CreateService();

        var group1 = new Group { IsSelected = true };
        var group2 = new Group { IsSelected = false };
        service.AddGroup(group1);
        service.AddGroup(group2);

        var args = new KeyboardEventArgs { Code = "Delete" };

        // Act
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));

        // Assert
        Assert.DoesNotContain(group1, service.Diagram.AllGroups);
        Assert.Contains(group2, service.Diagram.AllGroups);
    }

    [Fact]
    public void WhenDeletePressed_ShouldRemoveSelectedLinks()
    {
        // Arrange
        using var service = CreateService();
        var sourcePort = new Port();
        var targetPort = new Port();
        var group1 = new Group { IsSelected = true };
        var group2 = new Group { IsSelected = false };

        group1.Ports.Add(sourcePort);
        group2.Ports.Add(targetPort);
        var link1 = new Link() { IsSelected = true, SourcePort = sourcePort, TargetPort = targetPort };
        service.AddGroup(group1);
        service.AddGroup(group2);

        var args = new KeyboardEventArgs { Code = "Delete" };

        // Act
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));

        // Assert
        Assert.DoesNotContain(link1, service.Diagram.AllLinks);
    }

    [Fact]
    public void WhenDifferentKeyPressed_ShouldNotRemoveSelectedItems()
    {
        // Arrange
        using var service = CreateService();

        var node = new Node { IsSelected = true };
        var group = new Group { IsSelected = true };
        
        service.AddNode(node);
        service.AddGroup(group);
        var sourcePort = new Port();
        var targetPort = new Port();
        node.Ports.Add(sourcePort);
        group.Ports.Add(targetPort);
        var link = new Link();
        service.AddLinkTo(sourcePort, targetPort, link);
        link.IsSelected = true;

        var args = new KeyboardEventArgs { Code = "Backspace" };

        // Act
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));

        // Assert
        Assert.Contains(node, service.Diagram.AllNodes);
        Assert.Contains(group, service.Diagram.AllGroups);
        Assert.Contains(link, service.Diagram.AllLinks);
    }

    [Fact]
    public void WhenDeleteKeyCodeChanged_ShouldUseNewKeyCode()
    {
        // Arrange
        using var service = CreateService();
        service.Diagram.Options.Get<DeleteWithKeyOptions>().DeleteKeyCode = "Backspace";

        var node = new Node { IsSelected = true };
        service.AddNode(node);

        var args = new KeyboardEventArgs { Code = "Backspace" };

        // Act
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));

        // Assert
        Assert.DoesNotContain(node, service.Diagram.AllNodes);
    }

    [Fact]
    public void IsEnabled_WhenSetToFalse_ShouldNotProcessEvents()
    {
        // Arrange
        using var service = CreateService();
        service.Diagram.Options.Get<DeleteWithKeyOptions>().IsEnabled = false;


        var node = new Node { IsSelected = true };
        service.AddNode(node);

        var args = new KeyboardEventArgs { Code = "Delete" };

        // Act
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));

        // Assert
        Assert.Contains(node, service.Diagram.AllNodes);
    }

    [Fact]
    public void IsEnabled_WhenReEnabled_ShouldProcessEvents()
    {
        // Arrange
        using var service = CreateService();
        service.Diagram.Options.Get<DeleteWithKeyOptions>().IsEnabled = false;
        service.Diagram.Options.Get<DeleteWithKeyOptions>().IsEnabled = true;


        var node = new Node { IsSelected = true };
        service.AddNode(node);

        var args = new KeyboardEventArgs { Code = "Delete" };

        // Act
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));

        // Assert
        Assert.DoesNotContain(node, service.Diagram.AllNodes);
    }

    [Fact]
    public void Dispose_ShouldStopProcessingEvents()
    {
        // Arrange
        using var service = CreateService();

        var node = new Node { IsSelected = true };
        service.AddNode(node);

        var args = new KeyboardEventArgs { Code = "Delete" };

        // Verify initial behavior
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));
        Assert.DoesNotContain(node, service.Diagram.AllNodes);

        // Reset and dispose
        service.AddNode(node);
        service.Dispose();

        // Act
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));

        // Assert
        Assert.Contains(node, service.Diagram.AllNodes);
    }
}