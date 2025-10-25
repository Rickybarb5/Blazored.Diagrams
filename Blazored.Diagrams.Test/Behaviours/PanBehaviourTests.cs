using Blazored.Diagrams.Events;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Test.Behaviours;

public class PanBehaviourTests
{
    private IDiagramService Instance => CreateService();

    private IDiagramService CreateService()
    {
        var service = new DiagramService();
        return service;
    }

    [Fact]
    public void OnPanStart_WhenCtrlKeyNotPressed_ShouldInitiatePanning()
    {
        // Arrange
        using var obj = Instance;
        var args = new PointerEventArgs
        {
            CtrlKey = false,
            ClientX = 100,
            ClientY = 200
        };

        PanStartEvent? capturedEvent = null;
        obj.Events.SubscribeTo<PanStartEvent>(e => capturedEvent = e);

        // Act
        obj.Events.Publish(new DiagramPointerDownEvent(obj.Diagram, args));

        // Assert
        Assert.NotNull(capturedEvent);
        Assert.Equal(obj.Diagram, capturedEvent.Diagram);
    }

    [Fact]
    public void OnPanStart_WhenCtrlKeyPressed_ShouldNotInitiatePanning()
    {
        // Arrange
        using var obj = Instance;
        var args = new PointerEventArgs { CtrlKey = true };

        PanStartEvent? capturedEvent = null;
        obj.Events.SubscribeTo<PanStartEvent>(e => capturedEvent = e);

        // Act
        obj.Events.Publish(new DiagramPointerDownEvent(obj.Diagram, args));

        // Assert
        Assert.Null(capturedEvent);
    }

    [Fact]
    public void OnPan_WhenPanningIsActive_ShouldUpdateDiagramPosition()
    {
        // Arrange
        using var obj = Instance;
        var startArgs = new PointerEventArgs
        {
            CtrlKey = false,
            ClientX = 100,
            ClientY = 100
        };
        var moveArgs = new PointerEventArgs
        {
            ClientX = 150,
            ClientY = 150
        };

        // Start panning
        obj.Events.Publish(new DiagramPointerDownEvent(obj.Diagram, startArgs));

        // Act
        obj.Events.Publish(new DiagramPointerMoveEvent(obj.Diagram, moveArgs));

        // Assert
        Assert.Equal(50, obj.Diagram.PanX);
        Assert.Equal(50, obj.Diagram.PanY);
    }

    [Fact]
    public void OnPan_WhenPanningIsNotActive_ShouldNotUpdateDiagramPosition()
    {
        // Arrange
        using var obj = Instance;
        obj.Behaviours.GetBehaviourOptions<PanBehaviourOptions>().IsEnabled = false;
        var moveArgs = new PointerEventArgs
        {
            ClientX = 150,
            ClientY = 150
        };

        // Act - No pan start event published
        obj.Events.Publish(new DiagramPointerMoveEvent(obj.Diagram, moveArgs));

        // Assert
        Assert.Equal(0, obj.Diagram.PanX);
        Assert.Equal(0, obj.Diagram.PanY);
    }

    [Fact]
    public void OnPanEnd_WhenPanningIsActive_ShouldEndPanningAndPublishEvent()
    {
        // Arrange
        using var obj = Instance;
        var startArgs = new PointerEventArgs
        {
            CtrlKey = false,
            ClientX = 100,
            ClientY = 100
        };
        var endArgs = new PointerEventArgs();

        PanEndEvent? capturedEvent = null;
        obj.Events.SubscribeTo<PanEndEvent>(e => capturedEvent = e);

        // Start panning
        obj.Events.Publish(new DiagramPointerDownEvent(obj.Diagram, startArgs));

        // Act
        obj.Events.Publish(new DiagramPointerUpEvent(obj.Diagram, endArgs));

        // Assert
        Assert.NotNull(capturedEvent);
        Assert.Equal(obj.Diagram, capturedEvent.Diagram);
    }

    [Fact]
    public void OnPanEnd_WhenPanningIsNotActive_ShouldNotPublishEvent()
    {
        // Arrange
        using var obj = Instance;
        var endArgs = new PointerEventArgs();

        PanEndEvent? capturedEvent = null;
        obj.Events.SubscribeTo<PanEndEvent>(e => capturedEvent = e);

        // Act - No pan start event published
        obj.Events.Publish(new DiagramPointerUpEvent(obj.Diagram, endArgs));

        // Assert
        Assert.Null(capturedEvent);
    }

    [Fact]
    public void IsEnabled_WhenSetToFalse_ShouldNotProcessEvents()
    {
        // Arrange
        using var obj = Instance;
        obj.Behaviours.GetBehaviourOptions<PanBehaviourOptions>().IsEnabled = false;
        var startArgs = new PointerEventArgs
        {
            CtrlKey = false,
            ClientX = 100,
            ClientY = 100
        };

        PanStartEvent? capturedEvent = null;
        obj.Events.SubscribeTo<PanStartEvent>(e => capturedEvent = e);

        // Act
        obj.Events.Publish(new DiagramPointerDownEvent(obj.Diagram, startArgs));

        // Assert
        Assert.Null(capturedEvent);
    }

    [Fact]
    public void IsEnabled_WhenReEnabled_ShouldProcessEvents()
    {
        // Arrange
        using var obj = Instance;
        obj.Behaviours.GetBehaviourOptions<PanBehaviourOptions>().IsEnabled = false;
        obj.Behaviours.GetBehaviourOptions<PanBehaviourOptions>().IsEnabled = true;

        var startArgs = new PointerEventArgs
        {
            CtrlKey = false,
            ClientX = 100,
            ClientY = 100
        };

        PanStartEvent? capturedEvent = null;
        obj.Events.SubscribeTo<PanStartEvent>(e => capturedEvent = e);

        // Act
        obj.Events.Publish(new DiagramPointerDownEvent(obj.Diagram, startArgs));

        // Assert
        Assert.NotNull(capturedEvent);
    }

    // [Fact]
    // public void Dispose_ShouldStopProcessingEvents()
    // {
    //     // Arrange
    //     using var obj = Instance;
    //     var behaviour = obj.GetBehaviour<PanBehaviour>();
    //     behaviour!.Dispose();
    //     var startArgs = new PointerEventArgs
    //     {
    //         CtrlKey = false,
    //         ClientX = 100,
    //         ClientY = 100
    //     };
    //
    //     PanStartEvent? capturedEvent = null;
    //     obj.Events.SubscribeTo<PanStartEvent>(e => capturedEvent = e);
    //
    //     // Act
    //     obj.Events.Publish(new DiagramPointerDownEvent(obj.Diagram, startArgs));
    //
    //     // Assert
    //     Assert.Null(capturedEvent);
    // }
}