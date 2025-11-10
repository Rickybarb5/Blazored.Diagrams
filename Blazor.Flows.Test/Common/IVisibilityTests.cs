using Blazor.Flows.Groups;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Layers;
using Blazor.Flows.Links;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;

namespace Blazor.Flows.Test.Common;

public class VisibilityTests
{
    private static List<IVisible> GetModels()
    {
        return
        [
            new Node(),
            new Group(),
            new LineLink(),
            new Port(),
            new Layer()
        ];
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Test_Visibility(bool expected)
    {
        // Arrange
        var models = GetModels();
        foreach (var visible in models)
        {
            // Act
            visible.IsVisible = expected;

            //Assert
            Assert.Equal(expected, visible.IsVisible);
        }
    }
}