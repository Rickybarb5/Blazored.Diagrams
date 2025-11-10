using Blazor.Flows.Groups;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Links;
using Blazor.Flows.Nodes;

namespace Blazor.Flows.Test.Common;

public class SelectionTests
{
    private static List<ISelectable> GetModels()
    {
        return
        [
            new Node(),
            new Group(),
            new LineLink()
        ];
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Test_Selection(bool expected)
    {
        // Arrange
        var models = GetModels();
        foreach (var selectable in models)
        {
            // Act
            selectable.IsSelected = expected;

            //Assert
            Assert.Equal(expected, selectable.IsSelected);
        }
    }
}