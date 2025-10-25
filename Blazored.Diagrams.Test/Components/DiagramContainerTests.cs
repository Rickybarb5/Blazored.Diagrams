using Blazored.Diagrams.Components.Containers;
using Blazored.Diagrams.Events;
using Bunit;
using Moq;

namespace Blazored.Diagrams.Test.Components;

public class DiagramContainerTests : ContainerTestBase<DiagramContainer>
{
    public IRenderedComponent<DiagramContainer> CreateComponent()
    {
        var component = RenderComponent<DiagramContainer>(parameters => parameters
            .Add(p=> p.DiagramService, _diagramServiceMock.Object));

        return component;
    }

    [Fact]
    public void PublishesEvent_OnPointerDown()
    {
        // Arrange
        var cut = CreateComponent();

        // Act
        cut.Find("div").PointerDown();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<DiagramPointerDownEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerUp()
    {
        // Arrange
        var cut = CreateComponent();

        // Act
        cut.Find("div").PointerUp();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<DiagramPointerUpEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerEnter()
    {
        // Arrange
        var cut = CreateComponent();

        // Act
        cut.Find("div").PointerEnter();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<DiagramPointerEnterEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerLeave()
    {
        // Arrange
        var cut = CreateComponent();

        // Act
        cut.Find("div").PointerLeave();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<DiagramPointerLeaveEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerMove()
    {
        // Arrange
        var cut = CreateComponent();

        // Act
        cut.Find("div").PointerMove();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<DiagramPointerMoveEvent>()), Times.Once);
    }
    
    [Fact]
    public void PublishesEvent_OnClick()
    {
        // Arrange
        var cut = CreateComponent();

        // Act
        cut.Find("div").Click();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<DiagramClickedEvent>()), Times.Once);
    }
}