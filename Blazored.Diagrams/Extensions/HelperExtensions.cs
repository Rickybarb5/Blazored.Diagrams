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

    /// <summary>
    ///     Changes the position of a model to be in the center of a container.
    /// </summary>
    /// <param name="toCenter"></param>
    /// <param name="container"></param>
    /// <typeparam name="ToCenter">Model to be centered.</typeparam>
    /// <typeparam name="Container"></typeparam>
    public static void CenterIn<ToCenter, Container>(this ToCenter toCenter, Container container)
        where ToCenter : IPosition, ISize
        where Container : IPosition, ISize
    {
        // Calculate the center of the container
        var targetCenterX = container.PositionX + container.Width / 2;
        var targetCenterY = container.PositionY + container.Height / 2;

        // Calculate the new position for the toCenter element to center it within the container
        var newPositionX = targetCenterX - toCenter.Width / 2;
        var newPositionY = targetCenterY - toCenter.Height / 2;

        if (container is IPadding p)
        {
            newPositionX -= p.Padding;
            newPositionY -= p.Padding;
        }

        // Set the new position of the toCenter element
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