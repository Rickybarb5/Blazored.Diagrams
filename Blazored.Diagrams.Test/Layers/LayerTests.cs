using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Test.Layers;

public class LayerTests
{
    private Layer Instance => new();

    [Fact]
    public void Assert_Id_Is_Initialized()
    {
        //Arrange
        //Act
        var obj = Instance;

        //Assert
        Assert.NotEmpty(obj.Id);
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
    public void Node_Added_Triggers_Event()
    {
        //Arrange
        var layer = Instance;
        var node = new Node();
        var eventCount = 0;
        layer.OnNodeAddedToLayer.Subscribe(e =>
        {
            eventCount++;
            Assert.Same(node, e.Node);
            Assert.Same(layer, e.Model);
        });

        //Act
        layer.Nodes.AddInternal(node);

        //Assert
        Assert.Single(layer.Nodes);
        Assert.Same(node, layer.Nodes.First());
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Node_Removed_Triggers_Event()
    {
        //Arrange
        var layer = Instance;
        var node = new Node();
        var eventCount = 0;
        layer.OnNodeRemovedFromLayer.Subscribe(e =>
        {
            eventCount++;
            Assert.Same(node, e.Node);
            Assert.Same(layer, e.Model);
        });
        layer.Nodes.AddInternal(node);

        //Act
        layer.Nodes.RemoveInternal(node);

        //Assert
        Assert.Empty(layer.Nodes);
        Assert.Equal(1, eventCount);
    }


    [Fact]
    public void Group_Added_Triggers_Event()
    {
        //Arrange
        var layer = Instance;
        var added = new Group();
        var eventCount = 0;
        layer.OnGroupAddedToLayer.Subscribe(e =>
        {
            eventCount++;
            Assert.Same(added, e.AddedGroup);
            Assert.Same(layer, e.Model);
        });

        //Act
        layer.Groups.AddInternal(added);

        //Assert
        Assert.Single(layer.Groups);
        Assert.Same(added, layer.Groups.First());
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Group_Removed_Triggers_Event()
    {
        //Arrange
        var layer = Instance;
        var added = new Group();
        var eventCount = 0;
        layer.OnGroupRemovedFromLayer.Subscribe(e =>
        {
            eventCount++;
            Assert.Same(added, e.RemovedGroup);
            Assert.Same(layer, e.Model);
        });
        layer.Groups.AddInternal(added);

        //Act
        layer.Groups.RemoveInternal(added);

        //Assert
        Assert.Empty(layer.Groups);
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void Assert_AllGroups()
    {
        //Arrange
        var layer = Instance;
        var added = new Group();
        added.Groups.AddInternal(new Group());


        //Act
        layer.Groups.AddInternal(added);

        //Assert
        Assert.Equal(2, layer.AllGroups.Count);
    }

    [Fact]
    public void Assert_AllNodes()
    {
        //Arrange
        var layer = Instance;
        var group = new Group();
        group.Nodes.AddInternal(new Node());
        var node = new Node();


        //Act
        layer.Nodes.AddInternal(node);
        layer.Groups.AddInternal(group);

        //Assert
        Assert.Equal(2, layer.AllNodes.Count);
    }

    [Fact]
    public void Layer_UnselectAll_ShouldUnselectAllModels()
    {
        // Arrange
        var diagram = new Diagram();
        var layer = new Layer();
        var node1 = new Node { IsSelected = true };
        var node2 = new Node { IsSelected = true };
        var group = new Group { IsSelected = true };
        var sourcePort = new Port();
        var targetPort = new Port();

        node1.Ports.AddInternal(sourcePort);
        node2.Ports.AddInternal(targetPort);
        var link = new LineLink
        {
            IsSelected = true,
            SourcePort = sourcePort,
            TargetPort = targetPort
        };

        layer.Nodes.AddInternal(node1);
        layer.Nodes.AddInternal(node2);
        layer.Groups.AddInternal(group);
        diagram.Layers.AddInternal(layer);

        // Act
        layer.UnselectAll();

        // Assert
        Assert.False(node1.IsSelected);
        Assert.False(node2.IsSelected);
        Assert.False(group.IsSelected);
        Assert.False(link.IsSelected);
    }

    [Fact]
    public void Layer_SelectAll_ShouldSelectAllModels()
    {
        // Arrange
        var diagram = new Diagram();
        var layer = new Layer();
        var node1 = new Node { IsSelected = false };
        var node2 = new Node { IsSelected = false };
        var group = new Group { IsSelected = false };
        var sourcePort = new Port();
        var targetPort = new Port();

        node1.Ports.AddInternal(sourcePort);
        node2.Ports.AddInternal(targetPort);

        layer.Nodes.AddInternal(node1);
        layer.Nodes.AddInternal(node2);
        layer.Groups.AddInternal(group);
        diagram.Layers.AddInternal(layer);
        var link = new LineLink { SourcePort = sourcePort, TargetPort = targetPort };
        link.IsSelected = true;

        // Act
        layer.SelectAll();

        // Assert
        Assert.True(node1.IsSelected);
        Assert.True(node2.IsSelected);
        Assert.True(group.IsSelected);
        Assert.True(link.IsSelected);
    }

    [Fact]
    public void Layer_UnselectAll_ShouldUnselectNestedGroupContents()
    {
        // Arrange
        var diagram = new Diagram();
        var layer = new Layer();
        var group = new Group();
        var nestedGroup = new Group { IsSelected = true };
        var node = new Node { IsSelected = true };

        group.Groups.AddInternal(nestedGroup);
        group.Nodes.AddInternal(node);
        layer.Groups.AddInternal(group);
        diagram.Layers.AddInternal(layer);

        // Act
        layer.UnselectAll();

        // Assert
        Assert.False(group.IsSelected);
        Assert.False(nestedGroup.IsSelected);
        Assert.False(node.IsSelected);
    }

    [Fact]
    public void Layer_SelectAll_ShouldSelectNestedGroupContents()
    {
        // Arrange
        var diagram = new Diagram();
        var layer = new Layer();
        var group = new Group();
        var nestedGroup = new Group { IsSelected = false };
        var node = new Node { IsSelected = false };

        group.Groups.AddInternal(nestedGroup);
        group.Nodes.AddInternal(node);
        layer.Groups.AddInternal(group);
        diagram.Layers.AddInternal(layer);

        // Act
        layer.SelectAll();

        // Assert
        Assert.True(group.IsSelected);
        Assert.True(nestedGroup.IsSelected);
        Assert.True(node.IsSelected);
    }
}