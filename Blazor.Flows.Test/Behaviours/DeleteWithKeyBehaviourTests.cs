using Blazor.Flows.Events;
using Blazor.Flows.Groups;
using Blazor.Flows.Links;
using Blazor.Flows.Nodes;
using Blazor.Flows.Options.Behaviours;
using Blazor.Flows.Ports;
using Blazor.Flows.Services.Diagrams;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Flows.Test.Behaviours;

public class DeleteWithKeyBehaviourTests
{
    private IDiagramService CreateService()
    {
        var service = new DiagramService();
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

        service.AddPortTo(group1, sourcePort);
        service.AddPortTo(group2, targetPort);

        var link1 = new LineLink { IsSelected = true, SourcePort = sourcePort, TargetPort = targetPort };
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
        service.AddPortTo(node, sourcePort);
        service.AddPortTo(group, targetPort);
        var link = new LineLink();
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
        service.Behaviours.GetBehaviourOptions<DeleteWithKeyBehaviourOptions>().DeleteKeyCode = "Backspace";

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
        service.Behaviours.GetBehaviourOptions<DeleteWithKeyBehaviourOptions>().IsEnabled = false;


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
        service.Behaviours.GetBehaviourOptions<DeleteWithKeyBehaviourOptions>().IsEnabled = false;
        service.Behaviours.GetBehaviourOptions<DeleteWithKeyBehaviourOptions>().IsEnabled = true;


        var node = new Node { IsSelected = true };
        service.AddNode(node);

        var args = new KeyboardEventArgs { Code = "Delete" };

        // Act
        service.Events.Publish(new DiagramKeyDownEvent(service.Diagram, args));

        // Assert
        Assert.DoesNotContain(node, service.Diagram.AllNodes);
    }
}