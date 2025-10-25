using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Extensions;

/// <summary>
///     Helper extension methods.
/// </summary>
public static class HelperExtensions
{
    /// <summary>
    ///     Returns the center point of a model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static (int CenterX, int CenterY) GetCenterCoordinates<T>(this T model) where T : IPosition, ISize
    {
        return (model.PositionX + model.Width / 2, model.PositionY + model.Height / 2);
    }
    
    public static void CenterIn<ToCenter>(
        this ToCenter toCenter,
        int containerWidth,
        int containerHeight,
        int containerPositionX,
        int containerPositionY,
        int padding = 0)
        where ToCenter : IPosition, ISize
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
    /// <param name="diagram">The diagram instance.</param>
    /// <typeparam name="ToCenter">Type of the model to be centered.</typeparam>
    public static void CenterIn<ToCenter>(this ToCenter toCenter, IDiagram diagram)
        where ToCenter : IPosition, ISize
    {
        // Calculate the center of the viewport in world coordinates
        var viewportCenterX = (diagram.Width / 2 - diagram.PanX) / diagram.Zoom;
        var viewportCenterY = (diagram.Height / 2 - diagram.PanY) / diagram.Zoom;

        // Calculate the new position for the toCenter element to center it within the viewport
        var newPositionX = (int)(viewportCenterX - toCenter.Width / 2);
        var newPositionY = (int)(viewportCenterY - toCenter.Height / 2);

        // Set the new position of the toCenter element
        toCenter.SetPosition(newPositionX, newPositionY);
    }

    /// <summary>
    /// Disposes all subscriptions in a list.
    /// </summary>
    /// <param name="subscriptions"></param>
    public static void DisposeAll(this List<IDisposable> subscriptions)
    {
        subscriptions.ForEach(s => s.Dispose());
    }

    /// <summary>
    /// Applies an action to each item of an IEnumerable.
    /// </summary>
    /// <param name="list">List to iterate.</param>
    /// <param name="action">Action to perform</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        foreach (var item in list)
        {
            action(item);
        }
    }
}