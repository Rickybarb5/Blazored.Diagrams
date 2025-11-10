using Blazor.Flows.Groups;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;

namespace Blazor.Flows.Test.Common;

public class SizeTests
{
    private static List<ISize> GetModels()
    {
        return
        [
            new Node(),
            new Group(),
            new Port()
        ];
    }

    [Fact]
    public void Test_Set_Size()
    {
        // Arrange
        var models = GetModels();
        foreach (var sizeModel in models)
        {
            // Act
            sizeModel.SetSize(300, 600);

            //Assert
            Assert.Equal(300, sizeModel.Width);
            Assert.Equal(600, sizeModel.Height);
        }
    }

    [Fact]
    public void Test_Set_Width()
    {
        // Arrange
        var models = GetModels();
        foreach (var sizeModel in models)
        {
            // Act
            sizeModel.Width = 300;

            //Assert
            Assert.Equal(300, sizeModel.Width);
            Assert.Equal(0, sizeModel.Height);
        }
    }

    [Fact]
    public void Test_Set_Height()
    {
        // Arrange
        var models = GetModels();
        foreach (var sizeModel in models)
        {
            // Act
            sizeModel.Height = 300;

            //Assert
            Assert.Equal(300, sizeModel.Height);
            Assert.Equal(0, sizeModel.Width);
        }
    }
}