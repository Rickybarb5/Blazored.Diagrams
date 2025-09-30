using Blazored.Diagrams.Components.Containers;
using Blazored.Diagrams.Layers;
using Bunit;
using Moq;

namespace Blazored.Diagrams.Test.Components;

public class LayerContainerTests : ContainerTestBase<LayerContainer>
{
    public IRenderedComponent<LayerContainer> CreateComponent(ILayer layer)
    {
        var component = RenderComponent<LayerContainer>(parameters => parameters
            .Add(p => p.Layer, layer)
            .AddCascadingValue(_diagramServiceMock.Object));

        return component;
    }

    [Fact]
    public void DoesNotRender_WhenLayerIsNotVisible()
    {
        // Arrange
        var layer = new Layer { IsVisible = false };
        var cut = CreateComponent(layer);

        //Assert
        cut.MarkupMatches(string.Empty);
    }

    [Fact]
    public void RendersDiv_WhenLayerIsVisible()
    {
        // Arrange
        var layer = new Layer { IsVisible = true };
        var cut = CreateComponent(layer);

        //Act
        var value = cut.Find($"#{cut.Instance.ContainerId}");

        //Assert
        Assert.NotNull(value);
    }
}