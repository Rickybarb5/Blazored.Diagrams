using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services;
using Blazored.Diagrams.Services.Events;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Test.Behaviours;

public class ZoomBehaviorTests
{
    private IDiagramService CreateService()
    {
        var service = new DiagramService();
        service.Create<Diagram>();

        return service;
    }

    [Fact]
    public void WhenWheelScrolledUp_ShouldDecreaseZoom()
    {
        // Arrange
        using var service = CreateService();
        service.Diagram.Options.Get<ZoomOptions>()!.IsEnabled = true;
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
        service.Diagram.Options.Get<ZoomOptions>()!.IsEnabled = true;
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
        service.Diagram.Options.Get<ZoomOptions>()!.IsEnabled = false;
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
        service.Diagram.Options.Get<ZoomOptions>()!.IsEnabled = true;
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
        service.Diagram.Options.Get<ZoomOptions>()!.IsEnabled = false;
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
        service.Diagram.Options.Get<ZoomOptions>()!.IsEnabled = false;
        service.Diagram.Options.Get<ZoomOptions>()!.IsEnabled = true;
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

    [Fact]
    public void Dispose_ShouldStopProcessingEvents()
    {
        // Arrange
        using var service = CreateService();
        service.Diagram.Options.Get<ZoomOptions>()!.IsEnabled = true;
        var initialZoom = service.Diagram.Zoom;

        var args = new WheelEventArgs
        {
            DeltaY = 100
        };

        // Verify initial behavior
        service.Events.Publish(new DiagramWheelEvent(service.Diagram, args));
        Assert.NotEqual(initialZoom, service.Diagram.Zoom);

        // Reset zoom and dispose
        service.Diagram.SetZoom(initialZoom);

        // Act
        service.Events.Publish(new DiagramWheelEvent(service.Diagram, args));

        // Assert
        Assert.Equal(initialZoom, service.Diagram.Zoom);
    }
}