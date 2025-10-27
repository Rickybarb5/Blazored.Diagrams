using Blazored.Diagrams.Extensions;

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