using Blazored.Diagrams.Components.Containers;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Groups;
using Bunit;
using Moq;

namespace Blazored.Diagrams.Test.Components;

public class GroupContainerTests: ContainerTestBase<GroupContainer>
{
    public  IRenderedComponent<GroupContainer> CreateComponent(IGroup group)
    {
        return RenderComponent<GroupContainer>(parameters => parameters
            .Add(p => p.Group, group)
            .AddCascadingValue(_diagramServiceMock.Object));
    }
    
    [Fact]
    public void DoesNotRender_WhenGroupIsNotVisible()
    {
        // Arrange
        var group = new Group { IsVisible = false };
        var cut = CreateComponent(group);

        //Assert
        cut.MarkupMatches(string.Empty);
    }

    [Fact]
    public void RendersDiv_WhenGroupIsVisible()
    {
        // Arrange
        var group = new Group { PositionX = 100, PositionY = 200, IsVisible = true };
        var cut = CreateComponent(group);

        //Act
        var value = cut.Find($"#{cut.Instance.ContainerId}");
        
        //Assert
        Assert.NotNull(value);
    }

    [Fact]
    public void PublishesEvent_OnPointerDown()
    {
        // Arrange
        var group = new Group { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(group);

        // Act
        cut.Find("div").PointerDown();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<GroupPointerDownEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerUp()
    {
        // Arrange
        var group = new Group { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(group);
        
        // Act
        cut.Find("div").PointerUp();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<GroupPointerUpEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerEnter()
    {
        // Arrange
        var group = new Group { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(group);

        // Act
        cut.Find("div").PointerEnter();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<GroupPointerEnterEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerLeave()
    {
        // Arrange
        var group = new Group { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(group);

        // Act
        cut.Find("div").PointerLeave();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<GroupPointerLeaveEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnPointerMove()
    {
        // Arrange
        var group = new Group { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(group);

        // Act
        cut.Find("div").PointerMove();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<GroupPointerMoveEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnWheel()
    {
        // Arrange
        var group = new Group { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(group);

        // Act
        cut.Find("div").Wheel();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<GroupWheelEvent>()), Times.Once);
    }

    [Fact]
    public void PublishesEvent_OnClick()
    {
        // Arrange
        var group = new Group { PositionX = 0, PositionY = 0, IsVisible = true };
        var cut = CreateComponent(group);

        // Act
        cut.Find("div").Click();

        //Assert
        _eventServiceMock.Verify(e => e.Publish(It.IsAny<GroupClickedEvent>()), Times.Once);
    }
}
