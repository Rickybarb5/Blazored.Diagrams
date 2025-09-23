using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Services;

namespace Blazored.Diagrams.Test.Behaviours;

public class DefaultLayerBehaviourTests
{
    private IDiagramService CreateService()
    {
        var service = new DiagramService();
        service.Create<Diagram>();

        return service;
    }

    [Fact]
    public void HandleLayerAdded_WhenFirstLayer_ShouldSetAsCurrentLayer()
    {
        // Arrange
        using var service = CreateService();
        var layer = new Layer();

        // Act
        service.AddLayer(layer);

        // Assert
        Assert.True(service.Diagram.Layers.First().IsCurrentLayer);
    }

    [Fact]
    public void HandleLayerAdded_WhenNewLayerIsCurrentLayer_ShouldPublishEvent()
    {
        // Arrange
        using var service = CreateService();
        var secondLayer = new Layer { IsCurrentLayer = true };

        IsCurrentLayerChangedEvent? capturedEvent = null;
        service.Events.SubscribeTo<IsCurrentLayerChangedEvent>(e => capturedEvent = e);


        // Act
        service.AddLayer(secondLayer);

        // Assert
        Assert.NotNull(capturedEvent);
        Assert.Equal(secondLayer, capturedEvent.Model);
    }

    [Fact]
    public void HandleLayerSwitch_ShouldUpdateCurrentLayerStates()
    {
        // Arrange
        using var service = CreateService();
        var secondLayer = new Layer();

        service.AddLayer(secondLayer);

        // Act
        secondLayer.IsCurrentLayer = true;

        // Assert
        Assert.True(secondLayer.IsCurrentLayer);
    }

    [Fact]
    public void HandleLayerStateChange_WhenMultipleCurrentLayers_ShouldThrowException()
    {
        // Arrange
        using var service = CreateService();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.Diagram.Layers.First().IsCurrentLayer = false);
    }

    [Fact]
    public void HandleLayerRemoved_WhenLastLayer_ShouldThrowException()
    {
        // Arrange
        using var service = CreateService();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.Remove(service.Diagram.Layers.First()));
    }

    [Fact]
    public void HandleLayerRemoved_WhenCurrentLayerRemoved_ShouldSetFirstLayerAsCurrent()
    {
        // Arrange
        using var service = CreateService();
        var secondLayer = new Layer { IsCurrentLayer = true };

        service.AddLayer(secondLayer);

        // Act
        service.Remove(secondLayer);

        // Assert
        Assert.True(service.Diagram.Layers.First().IsCurrentLayer);
    }

    [Fact]
    public void Dispose_ShouldStopProcessingEvents()
    {
        // Arrange
        using var service = CreateService();
        var layer = new Layer { IsCurrentLayer = true };

        IsCurrentLayerChangedEvent? capturedEvent = null;
        service.Events.SubscribeTo<IsCurrentLayerChangedEvent>(e => capturedEvent = e);

        // Verify initial behavior
        service.AddLayer(layer);
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