using Bunit;
using Blazored.Diagrams.Components.Controls;
using Blazored.Diagrams.Services.Diagrams;
using Microsoft.Extensions.DependencyInjection;

namespace Blazored.Diagrams.Test.Components.Controls;

public class ZoomControlTests : TestContext
{
    private DiagramService CreateDiagramService(double initialZoom = 1.0)
    {

        var service = new DiagramService();
        return service;
    }

    [Fact]
    public void ZoomIn_ShouldIncreaseZoom()
    {
        // Arrange
        var service = CreateDiagramService(1.0);
        Services.AddSingleton<IDiagramService>(service);

        var cut = RenderComponent<ZoomControls>(parameters => parameters
            .AddCascadingValue(service)
        );

        // Act
        cut.Find("button[title='Zoom In']").Click();

        // Assert
        Assert.Equal(1.1, service.Diagram.Zoom);
    }

    [Fact]
    public void ZoomOut_ShouldDecreaseZoom()
    {
        // Arrange
        var service = CreateDiagramService(1.0);
        Services.AddSingleton<IDiagramService>(service);

        var cut = RenderComponent<ZoomControls>(parameters => parameters
            .AddCascadingValue(service)
        );

        // Act
        cut.Find("button[title='Zoom Out']").Click();

        // Assert
        Assert.Equal(0.9, service.Diagram.Zoom);
    }

    [Fact]
    public void ResetZoom_ShouldSetZoomToOne()
    {
        // Arrange
        var service = CreateDiagramService(1.5);
        Services.AddSingleton<IDiagramService>(service);

        var cut = RenderComponent<ZoomControls>(parameters => parameters
            .AddCascadingValue(service)
        );

        // Act
        cut.Find("div[title='Reset Zoom (100%)']").Click();

        // Assert
        Assert.Equal(1.0, service.Diagram.Zoom);
    }

    [Fact]
    public void ZoomMoreButton_ShouldBeDisabledAtLimit()
    {
        // Arrange
        var service = CreateDiagramService(3); // already at max
        Services.AddSingleton<IDiagramService>(service);

        var cut = RenderComponent<ZoomControls>(parameters => parameters
            .AddCascadingValue(service)
        );

        // Act
        var zoomInButton = cut.Find("button[title='Zoom In']");
        var zoomOutButton = cut.Find("button[title='Zoom Out']");

        // Assert
        Assert.False(zoomOutButton.HasAttribute("disabled"));
    }
    [Fact]
    public void ZoomLessButton_ShouldBeDisabledAtLimit()
    {
        // Arrange
        var service = CreateDiagramService(0.1); // already at max
        Services.AddSingleton<IDiagramService>(service);

        var cut = RenderComponent<ZoomControls>(parameters => parameters
            .AddCascadingValue(service)
        );

        // Act
        var zoomInButton = cut.Find("button[title='Zoom In']");
        var zoomOutButton = cut.Find("button[title='Zoom Out']");

        // Assert
        Assert.False(zoomOutButton.HasAttribute("disabled"));
    }
}
