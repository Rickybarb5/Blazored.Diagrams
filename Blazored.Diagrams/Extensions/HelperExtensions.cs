using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Extensions;

/// <summary>
///     Helper extension methods.
/// </summary>
public static class HelperExtensions
{
    /// <summary>
    /// Gets the bounds of a model.
    /// </summary>
    /// <param name="model"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Rect GetBounds<T>(this T model)
        where T : ISize, IPosition
    {
        return new()
        {
            Width = model.Width,
            Height = model.Height,
            Top = model.PositionY,
            Left = model.PositionX,
            Right = model.PositionX + model.Width,
            Bottom = model.PositionY + model.Height,
        };
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

    /// <summary>
    /// Gets the bounds from all components in the diagram.
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    public static IEnumerable<Rect> GetAllBounds(this IDiagramService service)
    {
       return service.Diagram
            .AllNodes
            .Select(x => x.GetBounds())
            .Concat(service.Diagram.AllGroups
                .Select(x => x.GetBounds()))
            .Concat(service.Diagram.AllPorts
                .Select(x => x.GetBounds()))
            .ToList();
    }
}