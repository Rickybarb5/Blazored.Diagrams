using Blazored.Diagrams.Events;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Services.Diagrams;


namespace Blazored.Diagrams.Test.Behaviours;

public class DefaultLayerBehaviourTests
{
    private IDiagramService CreateService()
    {
        var service = new DiagramService();
        return service;
    }

    [Fact]
    public void HandleLayerRemoved_WhenLastLayer_ShouldThrowException()
    {
        // Arrange
        using var service = CreateService();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.RemoveLayer(service.Diagram.Layers.First()));
    }

    [Fact]
    public void HandleLayerRemoved_WhenCurrentLayerRemoved_ShouldSetFirstLayerAsCurrent()
    {
        // Arrange
        using var service = CreateService();
        var secondLayer = new Layer();

        service.Diagram.CurrentLayer = secondLayer;

        // Act
        service.RemoveLayer(secondLayer);

        // Assert
        Assert.Same(service.Diagram.Layers.First(), service.Diagram.CurrentLayer);
    }

    [Fact]
    public void Dispose_ShouldStopProcessingEvents()
    {
        // Arrange
        using var service = CreateService();
        var layer = new Layer();
        CurrentLayerChangedEvent? capturedEvent = null;
        service.Events.SubscribeTo<CurrentLayerChangedEvent>(e => capturedEvent = e);

        // Verify initial behavior
        service.UseLayer(layer);
        Assert.NotNull(capturedEvent);

        // Reset and dispose
        capturedEvent = null;
        service.Dispose();

        // Act
        service.Events.Publish(new LayerAddedEvent(layer));

        // Assert
        Assert.Null(capturedEvent);
    }
}