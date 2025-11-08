using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Nodes;

namespace Blazored.Diagrams.Test.Common;

public class PositionTests
{
    private static List<IPosition> GetModels()
    {
        return
        [
            new Node(),
            new Group(),
        ];
    }

    [Fact]
    public void Test_SetPosition()
    {
        // Arrange
        var models = GetModels();
        foreach (var positionModel in models)
        {
            // Act
            positionModel.SetPosition(100, 350);

            //Assert
            Assert.Equal(100, positionModel.PositionX);
            Assert.Equal(350, positionModel.PositionY);
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
            sizeModel.PositionX = 300;

            //Assert
            Assert.Equal(300, sizeModel.PositionX);
            Assert.Equal(0, sizeModel.PositionY);
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
            sizeModel.PositionY = 300;

            //Assert
            Assert.Equal(300, sizeModel.PositionY);
            Assert.Equal(0, sizeModel.PositionX);
        }
    }
}