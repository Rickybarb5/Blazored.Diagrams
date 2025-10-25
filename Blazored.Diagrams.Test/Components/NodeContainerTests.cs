using Blazored.Diagrams.Components.Containers;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Nodes;

namespace Blazored.Diagrams.Test.Components;
using Bunit;
using Moq;
using Xunit;

public class NodeContainerTests : ContainerTestBase<NodeContainer>
{
    public  IRenderedComponent<NodeContainer> CreateComponent(INode node)
    {
        return RenderComponent<NodeContainer>(parameters => parameters
            .Add(p => p.Node, node)
            .AddCascadingValue(_diagramServiceMock.Object));
    }
    
    [Fact]
    public void DoesNotRender_WhenNodeIsNotVisible()
    {
        // Arrange
        var node = new Node { IsVisible = false };
        var cut = CreateComponent(node);

        //Assert
        cut.MarkupMatches(string.Empty);
    }

    [Fact]
    public void RendersDiv_WhenNodeIsVisible()
    {
        // Arrange
        var node = new Node { PositionX = 100, PositionY = 200, IsVisible = true };
        var cut = CreateComponent(node);

        //Act
        var value = cut.Find($"#{cut.Instance.ContainerId}");
        
        //Assert
        Assert.NotNull(value);
    }

    [Fact]
    public void PublishesEvent_OnPointerDown()
    {
        // Arrange
        var node = new Node { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(node);

        // Act
        cut.Find("div").PointerDown();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<NodePointerDownEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerUp()
    {
        // Arrange
        var node = new Node { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(node);
        
        // Act
        cut.Find("div").PointerUp();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<NodePointerUpEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerEnter()
    {
        // Arrange
        var node = new Node { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(node);

        // Act
        cut.Find("div").PointerEnter();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<NodePointerEnterEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerLeave()
    {
        // Arrange
        var node = new Node { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(node);

        // Act
        cut.Find("div").PointerLeave();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<NodePointerLeaveEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerMove()
    {
        // Arrange
        var node = new Node { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(node);

        // Act
        cut.Find("div").PointerMove();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<NodePointerMoveEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnWheel()
    {
        // Arrange
        var node = new Node { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(node);

        // Act
        cut.Find("div").Wheel();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<NodeWheelEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnClick()
    {
        // Arrange
        var node = new Node { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(node);

        // Act
        cut.Find("div").Click();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<NodeClickedEvent>()), Times.Once);
    }
}
