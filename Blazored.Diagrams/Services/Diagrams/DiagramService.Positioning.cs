using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Services.Diagrams;

public partial class DiagramService
{
      /// <summary>
    ///     Returns the center point of a model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public (int CenterX, int CenterY) GetCenterCoordinates<T>(T model) where T : ISize, IPosition
    {
        return (model.PositionX + model.Width / 2, model.PositionY + model.Height / 2);
    }
    
     /// <summary>
     /// Centers a model in another model
     /// </summary>
     /// <param name="toCenter"></param>
     /// <param name="containerWidth"></param>
     /// <param name="containerHeight"></param>
     /// <param name="containerPositionX"></param>
     /// <param name="containerPositionY"></param>
     /// <param name="padding"></param>
     /// <typeparam name="ToCenter"></typeparam>
    public void CenterTo<ToCenter>(
         ToCenter toCenter,
        int containerWidth,
        int containerHeight,
        int containerPositionX,
        int containerPositionY,
        int padding = 0)
        where ToCenter : ISize, IPosition
    {
        // --- 1. Calculate the inner content area (excluding padding) ---
        // The inner content's top-left corner starts after the padding.
        var innerPositionX = containerPositionX + padding;
        var innerPositionY = containerPositionY + padding;

        // The inner content's width/height is the total minus padding on both sides.
        var innerWidth = containerWidth - (padding * 2);
        var innerHeight = containerHeight - (padding * 2);

        // --- 2. Calculate the center of the inner content area ---
        var targetCenterX = innerPositionX + innerWidth / 2;
        var targetCenterY = innerPositionY + innerHeight / 2;

        // --- 3. Calculate the new position for 'toCenter' to align its center with the target center ---
        // This correctly accounts for the size of the object being centered.
        var newPositionX = targetCenterX - toCenter.Width / 2;
        var newPositionY = targetCenterY - toCenter.Height / 2;

        // --- 4. Set the new position ---
        // NO padding subtraction is needed here, as it was accounted for in Step 1 & 2.
        toCenter.SetPosition(newPositionX, newPositionY);
    }

    /// <summary>
    ///     Changes the position of a model to be in the center of the diagram, accounting for pan and zoom.
    /// </summary>
    /// <param name="toCenter">Model to be centered.</param>
    /// <typeparam name="ToCenter">Type of the model to be centered.</typeparam>
    public void CenterInViewport<ToCenter>(ToCenter toCenter)
        where ToCenter : IPosition, ISize
    {
        // Diagram might not be rendered on screen yet.
        if (Diagram.Width == 0 || Diagram.Height == 0)
        {
            return;
        }
        
        // Calculate the center of the viewport.
        var viewportCenterX = (Diagram.Width / 2 - Diagram.PanX) / Diagram.Zoom;
        var viewportCenterY = (Diagram.Height / 2 - Diagram.PanY) / Diagram.Zoom;
        
        // Calculate the new position for the toCenter element to center it within the viewport
        var newPositionX = (int)(viewportCenterX - toCenter.Width / 2);
        var newPositionY = (int)(viewportCenterY - toCenter.Height / 2);
        
        // Set the new position of the toCenter element
        toCenter.SetPosition(newPositionX, newPositionY);
    }
}