using Blazored.Diagrams.Components.Containers;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Ports;
using Bunit;
using Moq;

namespace Blazored.Diagrams.Test.Components;

public class PortContainerTests : ContainerTestBase<PortContainer>
{
    public IRenderedComponent<PortContainer> CreateComponent(IPort port)
    {
        return RenderComponent<PortContainer>(parameters => parameters
            .Add(p => p.Port, port)
            .AddCascadingValue(_diagramServiceMock.Object));
    }

    [Fact]
    public void DoesNotRender_WhenPortIsNotVisible()
    {
        // Arrange
        var port = new Port { IsVisible = false };
        var cut = CreateComponent(port);

        //Assert
        cut.MarkupMatches(string.Empty);
    }

    [Fact]
    public void RendersDiv_WhenPortIsVisible()
    {
        // Arrange
        var port = new Port { PositionX = 100, PositionY = 200, IsVisible = true };
        var cut = CreateComponent(port);

        //Act
        var value = cut.Find($"#{cut.Instance.ContainerId}");

        //Assert
        Assert.NotNull(value);
    }

    [Fact]
    public void PublishesEvent_OnPointerDown()
    {
        // Arrange
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").PointerDown();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<PortPointerDownEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerUp()
    {
        // Arrange
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").PointerUp();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<PortPointerUpEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerEnter()
    {
        // Arrange
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").PointerEnter();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<PortPointerEnterEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerLeave()
    {
        // Arrange
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").PointerLeave();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<PortPointerLeaveEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerMove()
    {
        // Arrange
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").PointerMove();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<PortPointerMoveEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnWheel()
    {
        // Arrange
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").Wheel();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<PortWheelEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnClick()
    {
        // Arrange
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").Click();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<PortClickedEvent>()), Times.Once);
    }
}