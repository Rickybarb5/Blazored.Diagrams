using Blazored.Diagrams.Components.Containers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Ports;
using Bunit;

namespace Blazored.Diagrams.Test.Components.Containers;

public class LinkContainerTests : ContainerTestBase<LinkContainer>
{
    public IRenderedComponent<LinkContainer> CreateComponent(ILink link)
    {
        var component = RenderComponent<LinkContainer>(parameters => parameters
            .Add(p => p.Link, link)
            .AddCascadingValue(_diagramServiceMock.Object));

        return component;
    }

    [Fact]
    public void DoesNotRender_WhenLinkIsNotVisible()
    {
        // Arrange
        var link = new LineLink { IsVisible = false };
        var cut = CreateComponent(link);

        //Assert
        cut.MarkupMatches(string.Empty);
    }

    [Fact]
    public void RendersDiv_WhenLinkIsVisible()
    {
        // Arrange
        var link = new CurvedLink { IsVisible = true, SourcePort = new Port()};
        var cut = CreateComponent(link);

        //Act
        var value = cut.Find($"#{cut.Instance.ContainerId}");

        //Assert
        Assert.NotNull(value);
    }
}