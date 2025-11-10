using Microsoft.AspNetCore.Components;

namespace Blazor.Flows.Services.Observers;

/// <summary>
/// Set of methods used by the <see cref="ResizeObserverService"/> class.
/// </summary>
public interface IResizeObserverService : IAsyncDisposable
{
    /// <summary>
    /// Starts observing the specified element by its id.
    /// </summary>
    /// <param name="elementId">Id of the html element.</param>
    /// <param name="callback">Action to run when the size changes.</param>
    /// <returns></returns>
    Task ObserveAsync(string elementId, Action<ResizeObserverEntry> callback);

    /// <summary>
    /// Starts observing the specified element using its reference.
    /// </summary>
    /// <param name="elementRef">Id of the html element.</param>
    /// <param name="callback">Action to run when the size changes.</param>
    /// <returns></returns>
    Task ObserveAsync(ElementReference elementRef, Action<ResizeObserverEntry> callback);

    /// <summary>
    /// Unregisters the observer by the html element id.
    /// </summary>
    /// <param name="elementId">Id of the html element.</param>
    /// <returns></returns>
    Task UnobserveAsync(string elementId);

    /// <summary>
    /// Unregisters the observer by the html element .
    /// </summary>
    /// <param name="elementRef">html element reference.</param>
    /// <returns></returns>
    Task UnobserveAsync(ElementReference elementRef);

    /// <summary>
    /// JSInvokable function that callbacks to user-defined code.
    /// </summary>
    /// <param name="elementId"></param>
    /// <param name="entry"></param>
    void OnResizeAsync(string elementId, ResizeObserverEntry entry);
}