using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Test.Nodes;

public class NodeTests
{
    private Node Instance => new();

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
        obj.OnSelectionChanged.Subscribe((e) => eventCount++);

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
        obj.OnVisibilityChanged.Subscribe((e) => eventCount++);

        //Act
        obj.IsVisible = false;

        //Assert
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Port_Added_Triggers_Event()
    {
        //Arrange
        var node = Instance;
        var added = new Port();
        var eventCount = 0;
        node.OnPortAddedToNode.Subscribe((e) => 
        {
            eventCount++;
            Assert.Same(added, e.Port);
            Assert.Same(node, e.Node);
        });

        //Act
        node.Ports.AddInternal(added);

        //Assert
        Assert.Single(node.Ports);
        Assert.Same(added, node.Ports.First());
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Port_Removed_Triggers_Event()
    {
        //Arrange
        var node = Instance;
        var port = new Port();
        var eventCount = 0;
        node.OnPortRemovedFromNode.Subscribe((e) =>
        {
            eventCount++;
            Assert.Same(port, e.Port);
            Assert.Same(node, e.Model);
        });
        node.Ports.AddInternal(port);

        //Act
        node.Ports.RemoveInternal(port);

        //Assert
        Assert.Empty(node.Ports);
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Test_Dispose()
    {
        // Arrange
        var obj = new Node();
        obj.Ports.AddInternal(new Port());

        //Act
        obj.Dispose();
        //Assert
        Assert.Empty(obj.Ports);
    }
}