using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Nodes;

namespace Blazored.Diagrams.Test.Helpers;

public class HelperTests
{
    [Fact]
    public void Test_Center_In()
    {
        //Arrange
        var group = new Group();
        var node = new Node();

        group.SetSize(100, 100);
        node.SetSize(50, 50);
        //Act
        node.CenterIn(group.Width, group.Height, group.PositionX, group.PositionY);
        //Assert
        Assert.NotEqual(0, node.PositionX);
        Assert.NotEqual(0, node.PositionY);
    }

    [Fact]
    public void Test_Center_In_Diagram()
    {
        //Arrange
        var diagram = new Diagram()
        {
            Width = 500,
            Height = 500,
        };
        var group = new Group();
        var node = new Node();

        group.SetSize(100, 100);
        node.SetSize(50, 50);
        //Act
        node.CenterIn(diagram);
        //Assert
        Assert.NotEqual(0, node.PositionX);
        Assert.NotEqual(0, node.PositionY);
    }

    [Fact]
    public void Test_Foreach()
    {
        //Arrange
        var list = Enumerable.Range(1, 20);
        var count = 0;
        //Act
        list.ForEach(n => count++);

        //Assert
        Assert.Equal(count, list.Count());
    }
}