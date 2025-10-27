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

    /*TODO:Add these afterwards
  [Fact]
 public void WhenParentNodeMoves_ShouldUpdatePortPosition()
 {
     // Arrange
     var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
     var cut = CreateComponent(port);

     var initialX = port.PositionX;
     var initialY = port.PositionY;

     // Act
     cut

     // Assert
     Assert.NotEqual(initialX, port.PositionX);
     Assert.NotEqual(initialY, port.PositionY);
 }

 [Fact]
 public void WhenIncomingLinkAdded_ShouldSetTargetPort()
 {
     // Arrange
     using var service = CreateService();
     var sourceNode = new Node();
     var targetNode = new Node();
     var sourcePort = new Port();
     var targetPort = new Port();
     service.Add.PortTo(sourceNode, sourcePort);
     service.Add.PortTo(targetNode, targetPort);
     service.Add.Node(sourceNode);
     service.Add.Node(targetNode);
     var link = new LineLink();

     // Act
     service.Add.AddLinkTo(sourcePort, targetPort, link);

     // Assert
     Assert.Equal(targetPort, link.TargetPort);
 }
 [Theory]
 [InlineData(PortAlignment.Left, PortJustification.Start)]
 [InlineData(PortAlignment.Left, PortJustification.End)]
 [InlineData(PortAlignment.Right, PortJustification.End)]
 [InlineData(PortAlignment.Right, PortJustification.Center)]
 [InlineData(PortAlignment.Right, PortJustification.Start)]
 [InlineData(PortAlignment.Top, PortJustification.Center)]
 [InlineData(PortAlignment.Top, PortJustification.Start)]
 [InlineData(PortAlignment.Top, PortJustification.End)]
 [InlineData(PortAlignment.Bottom, PortJustification.Start)]
 [InlineData(PortAlignment.Bottom, PortJustification.Center)]
 [InlineData(PortAlignment.Bottom, PortJustification.End)]
 [InlineData(PortAlignment.CenterParent, PortJustification.Center)]
 public void WhenPortAlignmentChanges_ShouldUpdatePosition(PortAlignment alignment, PortJustification justification)
 {
     // Arrange
     using var service = CreateService();
     var node = new Node { Width = 100, Height = 100, PositionX = 50, PositionY = 50 };
     var port = new Port { Width = 100, Height = 100 };
     service.Add.Node(node);
     service.Add.PortTo(node, port);

     var initialX = port.PositionX;
     var initialY = port.PositionY;

     // Act
     port.Alignment = alignment;
     port.Justification = justification;

     // Assert
     Assert.True(port.PositionX != initialX || port.PositionY != initialY);
 }
      [Fact]
          public void WhenParentNodeResizes_ShouldUpdatePortPosition()
          {
              // Arrange
              using var service = CreateService();
              var node = new Node { Width = 100, Height = 100 };
              var port = new Port { Width = 10, Height = 10, Alignment = PortAlignment.Right };
              service.Add.Node(node);
              service.Add.PortTo(node, port);

              var initialX = port.PositionX;

              // Act
              node.Width = 200;

              // Assert
              Assert.NotEqual(initialX, port.PositionX);
          }
 */
}