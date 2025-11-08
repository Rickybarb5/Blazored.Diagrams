using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Test.Groups;

public class GroupTests
{
    private Group Instance => new Group();

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
    public void Assert_Set_Padding()
    {
        // Arrange
        var obj = Instance;
        const int newPadding = 50;

        //Act
        obj.Padding = newPadding;

        //Assert
        Assert.Equal(newPadding, obj.Padding);
    }

    [Fact]
    public void Padding_Change_triggers_Event()
    {
        // Arrange
        var obj = Instance;
        var eventCount = 0;
        obj.OnPaddingChanged.Subscribe((e) => eventCount++);

        //Act
        obj.Padding = 50;

        //Assert
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Node_Added_Triggers_Event()
    {
        //Arrange
        var group = Instance;
        var node = new Node();
        var eventCount = 0;
        group.OnNodeAddedToGroup.Subscribe(e =>
        {
            eventCount++;
            Assert.Same(node, e.Node);
            Assert.Same(group, e.Model);
        });

        //Act
        group.Nodes.AddInternal(node);

        //Assert
        Assert.Single(group.Nodes);
        Assert.Same(node, group.Nodes.First());
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Node_Removed_Triggers_Event()
    {
        //Arrange
        var group = Instance;
        var node = new Node();
        var eventCount = 0;
        group.OnNodeRemovedFromGroup.Subscribe(e =>
        {
            eventCount++;
            Assert.Same(node, e.Node);
            Assert.Same(group, e.Model);
        });
        group.Nodes.AddInternal(node);

        //Act
        group.Nodes.RemoveInternal(node);

        //Assert
        Assert.Empty(group.Nodes);
        Assert.Equal(1, eventCount);
    }


    [Fact]
    public void Group_Added_Triggers_Event()
    {
        //Arrange
        var group = Instance;
        var added = new Group();
        var eventCount = 0;
        group.OnGroupAddedToGroup.Subscribe(e =>
        {
            eventCount++;
            Assert.Same(added, e.AddedGroup);
            Assert.Same(group, e.ParentModel);
        });

        //Act
        group.Groups.AddInternal(added);

        //Assert
        Assert.Single(group.Groups);
        Assert.Same(added, group.Groups.First());
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Group_Removed_Triggers_Event()
    {
        //Arrange
        var group = Instance;
        var added = new Group();
        var eventCount = 0;
        group.OnGroupRemovedFromGroup.Subscribe(e =>
        {
            eventCount++;
            Assert.Same(added, e.RemovedGroup);
            Assert.Same(group, e.ParentModel);
        });
        group.Groups.AddInternal(added);

        //Act
        group.Groups.RemoveInternal(added);

        //Assert
        Assert.Empty(group.Groups);
        Assert.Equal(1, eventCount);
    }


    [Fact]
    public void Port_Added_Triggers_Event()
    {
        //Arrange
        var group = Instance;
        var added = new Port();
        var eventCount = 0;
        group.OnPortAddedToGroup.Subscribe(e =>
        {
            eventCount++;
            Assert.Same(added, e.Port);
            Assert.Same(group, e.Model);
        });

        //Act
        group.Ports.AddInternal(added);

        //Assert
        Assert.Single(group.Ports);
        Assert.Same(added, group.Ports.First());
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Port_Removed_Triggers_Event()
    {
        //Arrange
        var group = Instance;
        var added = new Port();
        var eventCount = 0;
        group.OnPortRemovedFromGroup.Subscribe(e =>
        {
            eventCount++;
            Assert.Same(added, e.Port);
            Assert.Same(group, e.Model);
        });
        group.Ports.AddInternal(added);

        //Act
        group.Ports.RemoveInternal(added);

        //Assert
        Assert.Empty(group.Ports);
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Assert_AllPorts()
    {
        // Arrange
        //Act
        var obj = new Group();
        var node = new Node();
        var group = new Group();
        node.Ports.AddInternal(new Port());
        group.Ports.AddInternal(new Port());
        obj.Ports.AddInternal(new Port());
        obj.Nodes.AddInternal(node);
        obj.Groups.AddInternal(group);

        //Assert
        Assert.Equal(3, obj.AllPorts.Count);
    }

    [Fact]
    public void Assert_AllNodes()
    {
        // Arrange
        //Act
        var obj = new Group();
        var innerNode = new Node();
        var innerGroup = new Group();
        obj.Nodes.AddInternal(new Node());
        obj.Groups.AddInternal(innerGroup);
        innerGroup.Nodes.AddInternal(innerNode);

        //Assert
        Assert.Equal(2, obj.AllNodes.Count);
    }

    [Fact]
    public void Assert_AllGroups()
    {
        // Arrange
        //Act
        var obj = new Group();
        
        var nestedGroup = new Group();
        var lv2Group = new Group();
        obj.Groups.AddInternal(nestedGroup);
        nestedGroup.Groups.AddInternal(lv2Group);
        //Assert
        Assert.Equal(2, obj.AllGroups.Count);
    }

    [Fact]
    public void Test_Dispose()
    {
        // Arrange
        var obj = new Group();
        obj.Nodes.AddInternal(new Node());
        obj.Groups.AddInternal(new Group());
        obj.Ports.AddInternal(new Port());

        //Act
        obj.Dispose();
        //Assert
        Assert.Empty(obj.Ports);
        Assert.Empty(obj.Nodes);
        Assert.Empty(obj.Groups);
    }

    [Fact]
    public void Group_UnselectAll_ShouldUnselectAllNestedModels()
    {
        // Arrange
        var nestedGroup = new Group { IsSelected = true };
        var node = new Node { IsSelected = true };
        var parentGroup = new Group();
        
        parentGroup.Groups.AddInternal(nestedGroup);
        parentGroup.Nodes.AddInternal(node);
        // Act
        parentGroup.UnselectAll();

        // Assert
        Assert.False(nestedGroup.IsSelected);
        Assert.False(node.IsSelected);
    }

    [Fact]
    public void Group_SelectAll_ShouldSelectAllNestedModels()
    {
        // Arrange
        var nestedGroup = new Group { IsSelected = false };
        var node = new Node { IsSelected = false };
        var parentGroup = new Group();

        parentGroup.Groups.AddInternal(nestedGroup);
        parentGroup.Nodes.AddInternal(node);

        // Act
        parentGroup.SelectAll();

        // Assert
        Assert.True(nestedGroup.IsSelected);
        Assert.True(node.IsSelected);
    }
}