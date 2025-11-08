using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Test.Services;

public class DiagramServiceTests
{
    
    DiagramService Instance => new();
    [Fact]
    public void Test_Center_In()
    {
        //Arrange
        var service = Instance;
        service.Diagram.Width = 200;
        service.Diagram.Height = 200;
        var group = new Group();
        var node = new Node();
        var node2 = new Node();

        group.SetPosition(200, 200);
        node.SetPosition(50, 50);
        
        //Act
        service.CenterIn<IGroup, INode >(new (group, node));
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

        service.AddGroup(group).AddNode(node);
        group.SetSize(100, 100);
        node.SetSize(50, 50);
        //Act
        service.CenterInViewport(new CenterInViewportParameters<INode>(node));
        //Assert
        Assert.NotEqual(0, node.PositionX);
        Assert.NotEqual(0, node.PositionY);
    }
}