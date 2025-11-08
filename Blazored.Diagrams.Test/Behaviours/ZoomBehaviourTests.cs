using Blazored.Diagrams.Events;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Test.Behaviours;

public class ZoomBehaviorTests
{
    private IDiagramService CreateService()
    {
        var service = new DiagramService();
        return service;
    }

    [Fact]
    public void WhenWheelScrolledUp_ShouldDecreaseZoom()
    {
        // Arrange
        using var service = CreateService();
        service.Behaviours.GetBehaviourOptions<ZoomBehaviourOptions>()!.IsEnabled = true;
        var initialZoom = service.Diagram.Zoom;

        var args = new WheelEventArgs
        {
            DeltaY = 100 // Scroll up
        };

        // Act
        service.Events.Publish(new DiagramWheelEvent(service.Diagram, args));

        // Assert
        Assert.True(service.Diagram.Zoom < initialZoom);
    }

    [Fact]
    public void WhenWheelScrolledDown_ShouldIncreaseZoom()
    {
        // Arrange
        using var service = CreateService();
        service.Behaviours.GetBehaviourOptions<ZoomBehaviourOptions>()!.IsEnabled = true;
        var initialZoom = service.Diagram.Zoom;

        var args = new WheelEventArgs
        {
            DeltaY = -100 // Scroll down
        };

        // Act
        service.Events.Publish(new DiagramWheelEvent(service.Diagram, args));

        // Assert
        Assert.True(service.Diagram.Zoom > initialZoom);
    }

    [Fact]
    public void WhenZoomDisabled_ShouldNotChangeZoom()
    {
        // Arrange
        using var service = CreateService();
        service.Behaviours.GetBehaviourOptions<ZoomBehaviourOptions>()!.IsEnabled = false;
        var initialZoom = service.Diagram.Zoom;

        var args = new WheelEventArgs
        {
            DeltaY = 100 // Scroll up
        };

        // Act
        service.Events.Publish(new DiagramWheelEvent(service.Diagram, args));

        // Assert
        Assert.Equal(initialZoom, service.Diagram.Zoom);
    }

    [Fact]
    public void WhenDeltaYIsZero_ShouldNotChangeZoom()
    {
        // Arrange
        using var service = CreateService();
        service.Behaviours.GetBehaviourOptions<ZoomBehaviourOptions>()!.IsEnabled = true;
        var initialZoom = service.Diagram.Zoom;

        var args = new WheelEventArgs
        {
            DeltaY = 0
        };

        // Act
        service.Events.Publish(new DiagramWheelEvent(service.Diagram, args));

        // Assert
        Assert.Equal(initialZoom, service.Diagram.Zoom);
    }

    [Fact]
    public void IsEnabled_WhenSetToFalse_ShouldNotProcessEvents()
    {
        // Arrange
        using var service = CreateService();
        service.Behaviours.GetBehaviourOptions<ZoomBehaviourOptions>()!.IsEnabled = false;
        var initialZoom = service.Diagram.Zoom;

        var args = new WheelEventArgs
        {
            DeltaY = 100
        };

        // Act
        service.Events.Publish(new DiagramWheelEvent(service.Diagram, args));

        // Assert
        Assert.Equal(initialZoom, service.Diagram.Zoom);
    }

    [Fact]
    public void IsEnabled_WhenReEnabled_ShouldProcessEvents()
    {
        // Arrange
        using var service = CreateService();
        service.Behaviours.GetBehaviourOptions<ZoomBehaviourOptions>()!.IsEnabled = false;
        service.Behaviours.GetBehaviourOptions<ZoomBehaviourOptions>()!.IsEnabled = true;
        var initialZoom = service.Diagram.Zoom;

        var args = new WheelEventArgs
        {
            DeltaY = 100
        };

        // Act
        service.Events.Publish(new DiagramWheelEvent(service.Diagram, args));

        // Assert
        Assert.NotEqual(initialZoom, service.Diagram.Zoom);
    }
}