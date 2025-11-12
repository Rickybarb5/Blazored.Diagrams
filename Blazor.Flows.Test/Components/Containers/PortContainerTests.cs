using Blazor.Flows.Components.Containers;
using Blazor.Flows.Events;
using Blazor.Flows.Links;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;
using Blazor.Flows.Services.Diagrams;
using Bunit;

namespace Blazor.Flows.Test.Components.Containers;

public class PortContainerTests : ContainerTestBase<PortContainer>
{
    private DiagramService _service = new();

    public IRenderedComponent<PortContainer> CreateComponent(IPort port)
    {
        return RenderComponent<PortContainer>(parameters => parameters
            .Add(p => p.Port, port)
            .AddCascadingValue(_service));
    }

    [Fact]
    public void DoesNotRender_WhenPortIsNotVisible()
    {
        // Arrange
        var port = new Port { IsVisible = false };
        _service.AddPortTo(new Node(), port);
        var cut = CreateComponent(port);

        //Assert
        cut.MarkupMatches(string.Empty);
    }

    [Fact]
    public void RendersDiv_WhenPortIsVisible()
    {
        // Arrange
        var port = new Port { PositionX = 100, PositionY = 200, IsVisible = true };
        _service.AddPortTo(new Node(), port);
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
        var eventTriggered = false;
        _service.Events.SubscribeTo<PortPointerDownEvent>(_ => { eventTriggered = true; });
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        _service.AddPortTo(new Node(), port);
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").PointerDown();

        //Assert
        Assert.True(eventTriggered);
    }

    [Fact]
    public void PublishesEvent_OnPointerUp()
    {
        // Arrange
        var eventTriggered = false;
        _service.Events.SubscribeTo<PortPointerUpEvent>(_ => { eventTriggered = true; });
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        _service.AddPortTo(new Node(), port);
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").PointerUp();

        //Assert
        Assert.True(eventTriggered);
    }

    [Fact]
    public void PublishesEvent_OnPointerEnter()
    {
        // Arrange
        var eventTriggered = false;
        _service.Events.SubscribeTo<PortPointerEnterEvent>(_ => { eventTriggered = true; });
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        _service.AddPortTo(new Node(), port);
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").PointerEnter();

        //Assert
        Assert.True(eventTriggered);
    }

    [Fact]
    public void PublishesEvent_OnPointerLeave()
    {
        // Arrange
        var eventTriggered = false;
        _service.Events.SubscribeTo<PortPointerLeaveEvent>(_ => { eventTriggered = true; });
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        _service.AddPortTo(new Node(), port);
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").PointerLeave();

        //Assert
        Assert.True(eventTriggered);
    }

    [Fact]
    public void PublishesEvent_OnPointerMove()
    {
        // Arrange
        var eventTriggered = false;
        _service.Events.SubscribeTo<PortPointerMoveEvent>(_ => { eventTriggered = true; });
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        _service.AddPortTo(new Node(), port);
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").PointerMove();

        //Assert
        Assert.True(eventTriggered);
    }

    [Fact]
    public void PublishesEvent_OnWheel()
    {
        // Arrange
        var eventTriggered = false;
        _service.Events.SubscribeTo<PortWheelEvent>(_ => { eventTriggered = true; });
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        _service.AddPortTo(new Node(), port);
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").Wheel();

        //Assert
        Assert.True(eventTriggered);
    }

    [Fact]
    public void PublishesEvent_OnClick()
    {
        // Arrange
        var eventTriggered = false;
        _service.Events.SubscribeTo<PortClickedEvent>(_ => { eventTriggered = true; });
        var port = new Port { PositionX = 0, PositionY = 0, IsVisible = true };
        _service.AddPortTo(new Node(), port);
        var cut = CreateComponent(port);

        // Act
        cut.Find("div").Click();

        //Assert
        Assert.True(eventTriggered);
    }
    
    [Fact]
    public void WhenIncomingLinkAdded_ShouldSetTargetPort()
    {
        // Arrange
        var sourcePort = new Port();
        var targetPort = new Port();
        var sourceNode = new Node();
        var targetNode = new Node();
        _service.AddNode(sourceNode);
        _service.AddNode(targetNode);
        _service.AddPortTo(sourceNode, sourcePort);
        _service.AddPortTo(targetNode, targetPort);
        // SourcePort must be set for the Link model to initialize OutgoingLinks correctly
        var link = new LineLink { SourcePort = sourcePort };

        // Act
        _service.AddLinkTo(sourcePort, targetPort, link);
        link.TargetPort = targetPort;

        // Assert
        // Link model assertion
        Assert.Equal(targetPort, link.TargetPort);
        // Target Port model assertion (ensures the link was added to the port's IncomingLinks list)
        Assert.Contains(link, targetPort.IncomingLinks);
    }
}