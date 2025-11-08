using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Layers;

namespace Blazored.Diagrams.Test.Common;

public class GroupContainerTests
{
    private static List<IGroupContainer> GetModels()
    {
        return
        [
            new Group(),
            new Layer()
        ];
    }

    [Fact]
    public void Test_AddGroup()
    {
        // Arrange
        var containers = GetModels();
        var group = new Group();
        foreach (var container in containers)
        {
            // Act
            container.Groups.AddInternal(group);

            //Assert
            Assert.Same(group, container.Groups[0]);
            Assert.True(container.Groups.Count == 1);
        }
    }

    [Fact]
    public void Test_RemoveGroup()
    {
        // Arrange
        var containers = GetModels();
        var group = new Group();
        foreach (var container in containers)
        {
            container.Groups.AddInternal(group);
            // Act
            container.Groups.RemoveInternal(group);

            //Assert
            Assert.Empty(container.Groups);
        }
    }
}