using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Test.Common;

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