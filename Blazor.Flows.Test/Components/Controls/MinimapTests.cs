using Blazor.Flows.Components.Controls;
using Blazor.Flows.Nodes;
using Bunit;
using Blazor.Flows.Services.Diagrams;

namespace Blazor.Flows.Test.Components.Controls;

public class MinimapComponentTests : TestContext
{
    private readonly DiagramService _service;

    public MinimapComponentTests()
    {
        _service = new DiagramService();
        _service.Diagram.Width = 100;
        _service.Diagram.Height = 100;
    }

    [Fact]
    public void Renders_Svg_When_Service_Is_Provided()
    {
        // Arrange
        var cut = RenderComponent<MinimapControls>(parameters => parameters
            .AddCascadingValue(_service)
            .Add(p => p.Width, 200)
            .Add(p => p.Height, 150)
        );

        // Act
        var svg = cut.Find("svg");

        // Assert
        Assert.NotNull(svg);
        Assert.Equal("minimap-svg", svg.GetAttribute("class"));
        Assert.Equal("200", svg.GetAttribute("width"));
        Assert.Equal("150", svg.GetAttribute("height"));
    }

    [Fact]
    public void Renders_Node_Rectangles()
    {
        // Arrange
        _service.AddNode(new Node());
        var cut = RenderComponent<MinimapControls>(parameters => parameters
            .AddCascadingValue(_service)
            .Add(p => p.Width, 200)
            .Add(p => p.Height, 150)
            .Add(p => p.NodeColor, "red")
        );

        // Act
        var rect = cut.Find(".minimap-node");

        // Assert
        Assert.Equal("50", rect.GetAttribute("x"));
        Assert.Equal("50", rect.GetAttribute("y"));
        Assert.Equal("red", rect.GetAttribute("fill"));
    }
    

    [Fact]
    public void Shows_Pan_Info_Overlay()
    {
        // Arrange

        var cut = RenderComponent<MinimapControls>(parameters => parameters
            .AddCascadingValue(_service)
            .Add(p => p.Width, 200)
            .Add(p => p.Height, 150)
        );

        // Act
        var overlayText = cut.Find(".minimap-info-overlay").TextContent;

        // Assert
        Assert.Contains("(0", overlayText);
        Assert.Contains("0)", overlayText);
    }
}
