using Blazor.Flows.Components.Containers;
using Blazor.Flows.Layers;
using Bunit;

namespace Blazor.Flows.Test.Components.Containers;

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