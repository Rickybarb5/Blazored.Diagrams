using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;

namespace Blazored.Diagrams.Test.Common;

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