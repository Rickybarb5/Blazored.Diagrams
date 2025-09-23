using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Test.Common;

public class PortContainerTests
{
    private static List<IPortContainer> GetModels()
    {
        return
        [
            new Node(),
            new Group()
        ];
    }

    [Fact]
    public void Test_AddPort()
    {
        // Arrange
        var containers = GetModels();
        var port = new Port();
        foreach (var container in containers)
        {
            // Act
            container.Ports.Add(port);

            //Assert
            Assert.Same(port, container.Ports[0]);
            Assert.True(container.Ports.Count == 1);
        }
    }

    [Fact]
    public void Test_RemovePort()
    {
        // Arrange
        var containers = GetModels();
        var port = new Port();
        foreach (var container in containers)
        {
            container.Ports.Add(port);
            // Act
            container.Ports.Remove(port);

            //Assert
            Assert.Empty(container.Ports);
        }
    }
}