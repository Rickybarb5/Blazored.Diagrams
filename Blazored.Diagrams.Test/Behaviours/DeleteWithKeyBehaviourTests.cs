using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Diagrams;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Providers;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Test.Behaviours;

public class DeleteWithKeyBehaviourTests
{
    private IDiagramService CreateService()
    {
        DiagramServiceProvider diagramServiceProvider = new DiagramServiceProvider();
        var service = diagramServiceProvider.GetDiagramService(new Diagram());
        return service;
    }

    [Fact]
    public void WhenDeletePressed_ShouldRemoveSelectedNodes()
    {
        // Arrange
        using var service = CreateService();

        var node1 = new Node { IsSelected = true };
        var node2 = new Node { IsSelected = false };
        service.Add.Node(node1);
        service.Add.Node(node2);

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
        service.Add.Group(group1);
        service.Add.Group(group2);

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
        service.Add.Group(group1);
        service.Add.Group(group2);

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
        
        service.Add.Node(node);
        service.Add.Group(group);
        var sourcePort = new Port();
        var targetPort = new Port();
        node.Ports.Add(sourcePort);
        group.Ports.Add(targetPort);
        var link = new Link();
        service.Add.AddLinkTo(sourcePort, targetPort, link);
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
        service.Behaviours.GetBehaviourOptions<DeleteWithKeyBehaviourOptions>().DeleteKeyCode = "Backspace";

        var node = new Node { IsSelected = true };
        service.Add.Node(node);

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
        service.Behaviours.GetBehaviourOptions<DeleteWithKeyBehaviourOptions>().IsEnabled = false;


        var node = new Node { IsSelected = true };
        service.Add.Node(node);

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
        service.Behaviours.GetBehaviourOptions<DeleteWithKeyBehaviourOptions>().IsEnabled = false;
        service.Behaviours.GetBehaviourOptions<DeleteWithKeyBehaviourOptions>().IsEnabled = true;


        var node = new Node { IsSelected = true };
        service.Add.Node(node);

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
        service.Add.Node(node);

        var args = new KeyboardEventArgs { Code = "Delete" };

        // Verify initial behavior
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));
        Assert.DoesNotContain(node, service.Diagram.AllNodes);

        // Reset and dispose
        service.Add.Node(node);
        service.Dispose();

        // Act
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));

        // Assert
        Assert.Contains(node, service.Diagram.AllNodes);
    }
}