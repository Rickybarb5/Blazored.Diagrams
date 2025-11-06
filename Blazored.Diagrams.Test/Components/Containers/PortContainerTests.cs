using Blazored.Diagrams.Components.Containers;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Diagrams;
using Bunit;

namespace Blazored.Diagrams.Test.Components.Containers;

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
    public void WhenParentNodeMoves_ShouldUpdatePortPosition()
    {
        // Arrange
        var parentNode = new Node { PositionX = 100, PositionY = 100, Width = 50, Height = 50 };
        var port = new Port
        {
            Width = 10,
            Height = 10,
            Alignment = PortAlignment.Right,
            Justification = PortJustification.Center
        };
        _service.AddNode(parentNode);
        _service.AddPortTo(parentNode, port);
        var cut = CreateComponent(port);

        var initialX = port.PositionX;
        var initialY = port.PositionY;

        // Act
        parentNode.SetPosition(200, 200);
        cut.Render();

        // Assert
        Assert.NotEqual(initialX, port.PositionX);
        Assert.NotEqual(initialY, port.PositionY);
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

    [Theory]
    [InlineData(PortAlignment.Left, PortJustification.Start, PortAlignment.Right)]
    [InlineData(PortAlignment.Left, PortJustification.Center, PortAlignment.Right)]
    [InlineData(PortAlignment.Left, PortJustification.End, PortAlignment.Right)]
    [InlineData(PortAlignment.Right, PortJustification.End, PortAlignment.Left)]
    [InlineData(PortAlignment.Right, PortJustification.Center, PortAlignment.Left)]
    [InlineData(PortAlignment.Right, PortJustification.Start, PortAlignment.Left)]
    [InlineData(PortAlignment.Top, PortJustification.Center, PortAlignment.Bottom)]
    [InlineData(PortAlignment.Top, PortJustification.Start, PortAlignment.Bottom)]
    [InlineData(PortAlignment.Top, PortJustification.End, PortAlignment.Bottom)]
    [InlineData(PortAlignment.Bottom, PortJustification.Start, PortAlignment.Top)]
    [InlineData(PortAlignment.Bottom, PortJustification.Center, PortAlignment.Top)]
    [InlineData(PortAlignment.Bottom, PortJustification.End, PortAlignment.Top)]
    [InlineData(PortAlignment.CenterParent, PortJustification.Center, PortAlignment.Right)]
    public void WhenPortAlignmentChanges_ShouldUpdatePosition(PortAlignment alignment, PortJustification justification,
        PortAlignment alignmentTo)
    {
        // Arrange
        var parentNode = new Node { Width = 100, Height = 100, PositionX = 50, PositionY = 50 };
        var port = new Port
        {
            Width = 10,
            Height = 10,
            Alignment = alignment,
            Justification = justification
        };

        _service.AddNode(parentNode);
        _service.AddPortTo(parentNode, port);
        var cut = CreateComponent(port);

        var initialX = port.PositionX;
        var initialY = port.PositionY;

        // Act
        port.Alignment = alignmentTo;
        cut.Render();

        // Assert
        Assert.True(port.PositionX != initialX || port.PositionY != initialY,
            $"Port position did not update after changing alignment to {alignment}/{justification}.");
    }

    [Theory]
    [InlineData(PortAlignment.Left, PortJustification.Start, PortJustification.End)]
    [InlineData(PortAlignment.Left, PortJustification.Center, PortJustification.End)]
    [InlineData(PortAlignment.Left, PortJustification.End, PortJustification.Start)]
    [InlineData(PortAlignment.Right, PortJustification.End, PortJustification.Start)]
    [InlineData(PortAlignment.Right, PortJustification.Center, PortJustification.Start)]
    [InlineData(PortAlignment.Right, PortJustification.Start, PortJustification.End)]
    [InlineData(PortAlignment.Top, PortJustification.Center, PortJustification.Start)]
    [InlineData(PortAlignment.Top, PortJustification.Start, PortJustification.End)]
    [InlineData(PortAlignment.Top, PortJustification.End, PortJustification.Start)]
    [InlineData(PortAlignment.Bottom, PortJustification.Start, PortJustification.End)]
    [InlineData(PortAlignment.Bottom, PortJustification.Center, PortJustification.Start)]
    [InlineData(PortAlignment.Bottom, PortJustification.End, PortJustification.Start)]
    public void WhenPortJustificationChanges_ShouldUpdatePosition(PortAlignment alignment,
        PortJustification justification, PortJustification justificationTo)
    {
        // Arrange
        var parentNode = new Node { Width = 100, Height = 100, PositionX = 50, PositionY = 50 };
        var port = new Port
        {
            Width = 10,
            Height = 10,
            Alignment = alignment,
            Justification = justification
        };

        _service.AddNode(parentNode);
        _service.AddPortTo(parentNode, port);
        var cut = CreateComponent(port);

        var initialX = port.PositionX;
        var initialY = port.PositionY;

        // Act
        port.Justification = justificationTo;
        cut.Render();

        // Assert
        Assert.True(port.PositionX != initialX || port.PositionY != initialY,
            $"Port position did not update after changing justification to {alignment}/{justificationTo}.");
    }

    [Fact]
    public void WhenParentNodeResizes_ShouldUpdatePortPosition()
    {
        // Arrange
        var node = new Node { Width = 100, Height = 100, PositionX = 50, PositionY = 50 };
        var port = new Port
        {
            Width = 10,
            Height = 10,
            Alignment = PortAlignment.Right,
        };
        _service.AddNode(node);
        _service.AddPortTo(node, port);
        var cut = CreateComponent(port);

        var initialX = port.PositionX;

        // Act
        node.SetSize(200, 100);
        cut.Render();

        // Assert
        Assert.NotEqual(initialX, port.PositionX);
    }
}