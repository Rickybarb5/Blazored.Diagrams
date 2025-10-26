using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Nodes;

namespace Blazored.Diagrams.Test.Helpers;

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
}