using Blazor.Flows.Extensions;
using Blazor.Flows.Nodes;

namespace Blazor.Flows.Test.Helpers;

public class HelperTests
{
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

    [Fact]
    public void GetBounds_ShouldReturnCorrectRect()
    {
        // Arrange
        var model = new Node
        {
            Width = 100,
            Height = 50,
            PositionX = 10,
            PositionY = 20
        };

        // Act
        var rect = model.GetBounds();

        // Assert
        Assert.Equal(100, rect.Width);
        Assert.Equal(50, rect.Height);
        Assert.Equal(20, rect.Top);
        Assert.Equal(10, rect.Left);
        Assert.Equal(110, rect.Right);
        Assert.Equal(70, rect.Bottom);
    }
}