using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Test.Links;

public class LinkTests
{
    private Link Instance => new();

    [Fact]
    public void Assert_Id_Is_Initialized()
    {
        //Arrange
        //Act
        var obj = Instance;

        //Assert
        Assert.NotEqual(Guid.Empty, obj.Id);
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
        obj.OnSizeChanged += (_, _, _, _, _) => eventCount++;

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
        obj.TargetPositionX = newPosition;

        //Assert
        Assert.Equal(newPosition, obj.TargetPositionX);
    }

    [Fact]
    public void Assert_PositionY_Change()
    {
        // Arrange
        var obj = Instance;
        const int newPosition = 300;
        //Act
        obj.TargetPositionY = newPosition;

        //Assert
        Assert.Equal(newPosition, obj.TargetPositionY);
    }

    [Fact]
    public void Assert_Set_Position()
    {
        //Arrange
        var obj = Instance;
        const int posX = 100;
        const int posY = 300;

        //Act
        obj.SetTargetPosition(posX, posY);

        //Assert
        Assert.Equal(posX, obj.TargetPositionX);
        Assert.Equal(posY, obj.TargetPositionY);
    }

    [Fact]
    public void Assert_Position_Event_Triggered()
    {
        //Arrange
        var obj = Instance;
        const int width = 100;
        const int height = 300;
        var eventCount = 0;
        obj.OnTargetPositionChanged += (_, _, _, _, _) => eventCount++;

        //Act
        obj.TargetPositionX = 50;
        obj.TargetPositionY = 100;
        obj.SetTargetPosition(width, height);

        //Assert
        Assert.Equal(3, eventCount);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Assert_Set_Selection(bool expectedSelection)
    {
        // Arrange
        var obj = Instance;

        //Act
        obj.IsSelected = expectedSelection;

        //Assert
        Assert.Equal(expectedSelection, obj.IsSelected);
    }

    [Fact]
    public void Selection_Triggers_Event()
    {
        // Arrange
        var obj = Instance;
        var eventCount = 0;
        obj.OnSelectionChanged += (_) => eventCount++;

        //Act
        obj.IsSelected = true;

        //Assert
        Assert.Equal(1, eventCount);
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

    [Fact]
    public void Visibility_Triggers_Event()
    {
        // Arrange
        var obj = Instance;
        var eventCount = 0;
        obj.OnVisibilityChanged += (_) => eventCount++;

        //Act
        obj.IsVisible = false;

        //Assert
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Assert_Source_Port_Change()
    {
        // Arrange
        var obj = Instance;
        var sourcePort = new Port();

        //Act
        obj.SourcePort = sourcePort;

        //Assert
        Assert.Same(obj.SourcePort, sourcePort);
    }

    [Fact]
    public void Assert_Null_Source_Port_Throws_Exception()
    {
        // Arrange
        var obj = Instance;

        //Act
        var action = () => obj.SourcePort = null;

        //Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void Assert_Target_Port_Change()
    {
        // Arrange
        var obj = Instance;
        var targetPort = new Port() { Parent = new Node() };

        //Act
        obj.TargetPort = targetPort;

        //Assert
        Assert.Same(obj.TargetPort, targetPort);
    }

    [Fact]
    public void Source_Port_Change_Triggers_Event()
    {
        // Arrange
        var obj = Instance;
        var eventCount = 0;
        obj.OnSourcePortChanged += (_, _, _) => eventCount++;

        //Act
        obj.SourcePort = new Port();

        //Assert
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Target_Port_Change_Triggers_Event()
    {
        // Arrange
        var obj = Instance;
        var eventCount = 0;
        obj.OnTargetPortChanged += (_, _, _) => eventCount++;

        //Act
        obj.TargetPort = new Port() { Parent = new Node() };

        //Assert
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Test_Is_Connected()
    {
        // Arrange
        var obj = Instance;
        var source = new Port();
        var target = new Port();
        obj.SourcePort = source;
        obj.TargetPort = target;

        //Act
        //Assert
        Assert.True(obj.IsConnected);
    }

    [Fact]
    public void Test_Is_Not_Connected()
    {
        // Arrange
        var obj = Instance;
        var source = new Port();
        obj.SourcePort = source;

        //Act
        //Assert
        Assert.False(obj.IsConnected);
    }

    [Fact]
    public void Test_SetTargetPosition()
    {
        // Arrange
        var obj = Instance;
        var source = new Port();
        var target = new Port { PositionX = 100, PositionY = 200 };
        obj.SourcePort = source;

        //Act
        obj.SetTargetPosition(target);
        //Assert
        var coordinates = target.GetCenterCoordinates();
        Assert.Equal(coordinates.CenterX, obj.TargetPositionX);
        Assert.Equal(coordinates.CenterY, obj.TargetPositionY);
    }

    [Fact]
    public void Test_Dispose()
    {
        // Arrange
        var obj = Instance;
        var source = new Port();
        var target = new Port();
        obj.SourcePort = source;
        obj.TargetPort = target;

        //Act
        obj.Dispose();
        //Assert
        Assert.Empty(source.OutgoingLinks);
        Assert.Empty(target.IncomingLinks);
    }
}