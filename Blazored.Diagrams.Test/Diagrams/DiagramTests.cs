using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Test.Diagrams;

public class DiagramTests
{
    private Diagram Instance => new();

    [Fact]
    public void Assert_Default_Layer_Exists()
    {
        //Arrange
        //Act
        var diagram = Instance;

        //Assert
        Assert.Same(diagram.CurrentLayer, diagram.Layers.First());
        Assert.NotNull(diagram.CurrentLayer);
    }

    [Fact]
    public void Test_Set_Zoom()
    {
        //Arrange
        const double zoom = 2;
        var diagram = Instance;

        //Act
        diagram.Zoom = zoom;

        //Assert
        Assert.Equal(zoom, diagram.Zoom);
    }

    [Fact]
    public void Test_Set_Pan()
    {
        //Arrange
        const int panX = 2;
        const int panY = 2;
        var diagram = Instance;

        //Act
        diagram.SetPan(panX, panY);

        //Assert
        Assert.Equal(panX, diagram.PanX);
        Assert.Equal(panY, diagram.PanY);
    }

    [Fact]
    public void Test_Set_PanX()
    {
        //Arrange
        const int panX = 2;
        var diagram = Instance;

        //Act
        diagram.PanX = panX;

        //Assert
        Assert.Equal(panX, diagram.PanX);
    }

    [Fact]
    public void Test_Set_PanY()
    {
        //Arrange
        const int panY = 2;
        var diagram = Instance;

        //Act
        diagram.PanY = panY;

        //Assert
        Assert.Equal(panY, diagram.PanY);
    }

    [Fact]
    public void Assert_Layer_Events_Triggered()
    {
        //Arrange
        var diagram = Instance;
        var layer = new Layer();
        var addEventCount = 0;
        var removeEventCount = 0;
        diagram.OnLayerAdded.Subscribe((e) => addEventCount++);
        diagram.OnLayerRemoved.Subscribe((e) => removeEventCount++);

        //Act
        diagram.Layers.AddInternal(layer);
        diagram.Layers.RemoveInternal(layer);

        //Assert
        Assert.Equal(1, addEventCount);
        Assert.Equal(1, removeEventCount);
    }

    [Fact]
    public void Assert_Pan_Events_Triggered()
    {
        //Arrange
        var diagram = Instance;
        var eventCount = 0;
        diagram.OnPanChanged.Subscribe((e) => eventCount++);

        //Act
        diagram.PanX = 5;
        diagram.PanY = 10;
        diagram.SetPan(20, 10);

        //Assert
        Assert.Equal(3, eventCount);
    }

    [Fact]
    public void Diagram_UnselectAll_ShouldUnselectAllModelsInAllLayers()
    {
        // Arrange
        var diagram = new Diagram();
        var layer1 = new Layer();
        var layer2 = new Layer();
        var node1 = new Node { IsSelected = true };
        var node2 = new Node { IsSelected = true };
        var sourcePort = new Port();
        var targetPort = new Port();

        node1.Ports.AddInternal(sourcePort);
        node2.Ports.AddInternal(targetPort);

        layer1.Nodes.AddInternal(node1);
        layer2.Nodes.AddInternal(node2);

        diagram.Layers.AddInternal(layer1);
        diagram.Layers.AddInternal(layer2);
        var link = new LineLink { SourcePort = sourcePort, TargetPort = targetPort };
        link.IsSelected = true;

        // Act
        diagram.UnselectAll();

        // Assert
        Assert.False(node1.IsSelected);
        Assert.False(node2.IsSelected);
        Assert.False(link.IsSelected);
    }

    [Fact]
    public void Diagram_SelectAll_ShouldSelectAllModelsInAllLayers()
    {
        // Arrange
        var diagram = new Diagram();
        var layer1 = new Layer();
        var layer2 = new Layer();
        var node1 = new Node { IsSelected = false };
        var node2 = new Node { IsSelected = false };
        var sourcePort = new Port();
        var targetPort = new Port();

        node1.Ports.AddInternal(sourcePort);
        node2.Ports.AddInternal(targetPort);

        layer1.Nodes.AddInternal(node1);
        layer2.Nodes.AddInternal(node2);

        diagram.Layers.AddInternal(layer1);
        diagram.Layers.AddInternal(layer2);
        var link = new LineLink { SourcePort = sourcePort, TargetPort = targetPort };
        link.IsSelected = true;

        // Act
        diagram.SelectAll();

        // Assert
        Assert.True(node1.IsSelected);
        Assert.True(node2.IsSelected);
        Assert.True(link.IsSelected);
    }
}