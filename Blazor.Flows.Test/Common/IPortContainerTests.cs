using Blazor.Flows.Groups;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;

namespace Blazor.Flows.Test.Common;

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
            container.Ports.AddInternal(port);

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
            container.Ports.AddInternal(port);
            // Act
            container.Ports.RemoveInternal(port);

            //Assert
            Assert.Empty(container.Ports);
        }
    }
}