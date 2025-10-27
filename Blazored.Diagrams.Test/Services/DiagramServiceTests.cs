using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Test.Services;

public class DiagramServiceTests
{
    
    DiagramService Instance => new DiagramService();
    [Fact]
    public void Test_Center_In()
    {
        //Arrange
        var service = Instance;
        var group = new Group();
        var node = new Node();

        group.SetSize(100, 100);
        node.SetSize(50, 50);
        //Act
        service.CenterTo(node, group.Width, group.Height, group.PositionX, group.PositionY);
        //Assert
        Assert.NotEqual(0, node.PositionX);
        Assert.NotEqual(0, node.PositionY);
    }

    [Fact]
    public void Test_Center_In_Diagram()
    {
        //Arrange
        var service = Instance;
        var group = new Group();
        var node = new Node();
        service.Diagram.Width = 100;
        service.Diagram.Height = 100;

        group.SetSize(100, 100);
        node.SetSize(50, 50);
        //Act
        service.CenterInViewport(node);
        //Assert
        Assert.NotEqual(0, node.PositionX);
        Assert.NotEqual(0, node.PositionY);
    }
}