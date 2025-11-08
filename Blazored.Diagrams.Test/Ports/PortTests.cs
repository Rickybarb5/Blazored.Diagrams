using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Test.Ports;

public class PortTests
{
    private Port Instance => new Port { Parent = new Node() };

    [Fact]
    public void Assert_Id_Is_Initialized()
    {
        //Arrange
        //Act
        var obj = Instance;

        //Assert
        Assert.NotEmpty(obj.Id);
    }

    [Fact]
    public void Assert_Width_Change()
    {
        //Arrange
        var obj = Instance;
        var width = 300;
        //Act
        obj.Width = width;

        //Assert
        Assert.Equal(width, obj.Width);
    }

    [Fact]
    public void Assert_Height_Change()
    {
        //Arrange
        var obj = Instance;
        var height = 300;
        //Act
        obj.Height = height;

        //Assert
        Assert.Equal(height, obj.Height);
    }

    [Fact]
    public void Assert_Set_Size()
    {
        //Arrange
        var obj = Instance;
        var width = 100;
        var height = 300;
        //Act
        obj.SetSize(width, height);

        //Assert
        Assert.Equal(height, obj.Height);
        Assert.Equal(width, obj.Width);
    }

    [Fact]
    public void Assert_Size_Event_Triggered()
    {
        //Arrange
        var obj = Instance;
        const int width = 100;
        const int height = 300;
        var eventCount = 0;
        obj.OnSizeChanged.Subscribe((e) => eventCount++);

        //Act
        obj.Width = 50;
        obj.Height = 100;
        obj.SetSize(width, height);

        //Assert
        Assert.Equal(3, eventCount);
    }

    [Fact]
    public void Assert_PositionX_Change()
    {
        //Arrange
        var obj = Instance;
        const int newPosition = 300;
        //Act
        obj.PositionX = newPosition;

        //Assert
        Assert.Equal(newPosition, obj.PositionX);
    }

    [Fact]
    public void Assert_PositionY_Change()
    {
        // Arrange
        var obj = Instance;
        const int newPosition = 300;
        //Act
        obj.PositionY = newPosition;

        //Assert
        Assert.Equal(newPosition, obj.PositionY);
    }

    [Fact]
    public void Assert_Set_Position()
    {
        //Arrange
        var obj = Instance;
        const int posX = 100;
        const int posY = 300;

        //Act
        obj.SetPosition(posX, posY);

        //Assert
        Assert.Equal(posX, obj.PositionX);
        Assert.Equal(posY, obj.PositionY);
    }

    [Fact]
    public void Assert_Position_Event_Triggered()
    {
        //Arrange
        var obj = Instance;
        const int width = 100;
        const int height = 300;
        var eventCount = 0;
        obj.OnPositionChanged.Subscribe((e) => eventCount++);

        //Act
        obj.PositionX = 50;
        obj.PositionY = 100;
        obj.SetPosition(width, height);

        //Assert
        Assert.Equal(3, eventCount);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Assert_Set_Visibility(bool expectedVisibility)
    {
        // Arrange
        var obj = Instance;

        //Act
        obj.IsVisible = expectedVisibility;

        //Assert
        Assert.Equal(expectedVisibility, obj.IsVisible);
    }

    [Theory]
    [InlineData(PortJustification.Center)]
    [InlineData(PortJustification.Start)]
    [InlineData(PortJustification.End)]
    public void Assert_Set_PortJustification(PortJustification justification)
    {
        // Arrange
        var obj = Instance;

        //Act
        obj.Justification = justification;

        //Assert
        Assert.Equal(justification, obj.Justification);
    }

    [Fact]
    public void Justification_Change_Triggers_Event()
    {
        // Arrange
        var obj = Instance;
        var eventCount = 0;
        obj.OnPortJustificationChanged.Subscribe((e) => eventCount++);

        //Act
        obj.Justification = PortJustification.End;

        //Assert
        Assert.Equal(1, eventCount);
    }

    [Theory]
    [InlineData(PortAlignment.Left)]
    [InlineData(PortAlignment.Right)]
    [InlineData(PortAlignment.Bottom)]
    [InlineData(PortAlignment.Top)]
    [InlineData(PortAlignment.CenterParent)]
    public void Assert_Set_PortAlignment(PortAlignment alignment)
    {
        // Arrange
        var obj = Instance;

        //Act
        obj.Alignment = alignment;

        //Assert
        Assert.Equal(alignment, obj.Alignment);
    }

    [Fact]
    public void Alignment_Change_Triggers_Event()
    {
        // Arrange
        var obj = Instance;
        var count = 0;
        obj.OnPortAlignmentChanged.Subscribe((e) => count++);

        //Act
        obj.Alignment = PortAlignment.CenterParent;

        //Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void Visibility_Triggers_Event()
    {
        // Arrange
        var obj = Instance;
        var eventCount = 0;
        obj.OnVisibilityChanged.Subscribe((e) => eventCount++);

        //Act
        obj.IsVisible = false;

        //Assert
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void CanConnectTo_Default_Behaviour_Does_Not_Connect_To_Target_Port()
    {
        //Arrange
        var node = new Node();
        var sourcePort = new Port { Parent = node };
        var targetPort = new Port { Parent = node };
        node.Ports.AddInternal(sourcePort);
        node.Ports.AddInternal(targetPort);

        // Act
        var result = sourcePort.CanConnectTo(targetPort);
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanConnectTo_Default_Behaviour_Connects_To_Target_Port()
    {
        //Arrange
        var node = new Node();
        var node2 = new Node();
        var sourcePort = new Port();
        var targetPort = new Port();
        node.Ports.AddInternal(sourcePort);
        node2.Ports.AddInternal(targetPort);

        // Act
        var result = sourcePort.CanConnectTo(targetPort);
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Test_Dispose()
    {
        //Arrange
        var port = new Port { Parent = new Node() };
        // Act
        port.Dispose();
        // Assert
        Assert.Empty(port.IncomingLinks);
        Assert.Empty(port.OutgoingLinks);
        Assert.Empty(port.Parent.Ports);
    }

    [Fact]
    public void Assert_Has_Outgoing_Links()
    {
        //Arrange
        var port = new Port();
        // Act
        port.OutgoingLinks.AddInternal(new LineLink());
        // Assert
        Assert.True(port.HasOutGoingLinks);
        Assert.True(port.HasLinks);
    }

    [Fact]
    public void Assert_Has_Incoming_Links()
    {
        //Arrange
        var port = new Port();
        // Act
        port.IncomingLinks.AddInternal(new LineLink());
        // Assert
        Assert.True(port.HasIncomingLinks);
        Assert.True(port.HasLinks);
    }

    [Fact]
    public void Test_CanCreateLink_Is_True_By_Default()
    {
        //Arrange
        var port = new Port();
        // Act
        var result = port.CanCreateLink();
        // Assert
        Assert.True(result);
    }
}