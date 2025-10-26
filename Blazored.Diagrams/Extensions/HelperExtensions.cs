using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Extensions;

/// <summary>
///     Helper extension methods.
/// </summary>
public static class HelperExtensions
{
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