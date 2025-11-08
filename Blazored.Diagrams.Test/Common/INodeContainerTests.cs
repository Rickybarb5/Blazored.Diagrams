using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Nodes;

namespace Blazored.Diagrams.Test.Common;

public class NodeContainerTests
{
    private static List<INodeContainer> GetModels()
    {
        return
        [
            new Group(),
            new Layer(),
        ];
    }

    [Fact]
    public void Test_AddNode()
    {
        // Arrange
        var containers = GetModels();
        var node = new Node();
        foreach (var container in containers)
        {
            // Act
            container.Nodes.AddInternal(node);

            //Assert
            Assert.Same(node, container.Nodes[0]);
            Assert.True(container.Nodes.Count == 1);
        }
    }

    [Fact]
    public void Test_RemoveNode()
    {
        // Arrange
        var containers = GetModels();
        var node = new Node();
        foreach (var container in containers)
        {
            container.Nodes.AddInternal(node);
            // Act
            container.Nodes.RemoveInternal(node);

            //Assert
            Assert.Empty(container.Nodes);
        }
    }
}