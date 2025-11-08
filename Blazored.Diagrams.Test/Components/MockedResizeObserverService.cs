namespace Blazored.Diagrams.Test.Components;

using Moq;
using Blazored.Diagrams.Services.Observers;
using Microsoft.AspNetCore.Components;

/// <summary>
///     Helper class for creating and working with Moq mocks of <see cref="IResizeObserverService"/>.
/// </summary>
public class ResizeObserverServiceMock
{
    private readonly Dictionary<string, Action<ResizeObserverEntry>> _callbacks = new();

    public Mock<IResizeObserverService> Mock { get; }
    public IResizeObserverService Object => Mock.Object;

    public ResizeObserverServiceMock()
    {
        Mock = new Mock<IResizeObserverService>();
        SetupMock();
    }

    private void SetupMock()
    {
        // Setup ObserveAsync(string, Action)
        Mock.Setup(x => x.ObserveAsync(It.IsAny<string>(), It.IsAny<Action<ResizeObserverEntry>>()))
            .Returns(Task.CompletedTask)
            .Callback<string, Action<ResizeObserverEntry>>((id, callback) => _callbacks[id] = callback);

        // Setup ObserveAsync(ElementReference, Action)
        Mock.Setup(x => x.ObserveAsync(It.IsAny<ElementReference>(), It.IsAny<Action<ResizeObserverEntry>>()))
            .Returns(Task.CompletedTask)
            .Callback<ElementReference, Action<ResizeObserverEntry>>((elementRef, callback) =>
                _callbacks[$"element-{elementRef.Id}"] = callback);

        // Setup UnobserveAsync(string)
        Mock.Setup(x => x.UnobserveAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask)
            .Callback<string>(id => _callbacks.Remove(id));

        // Setup UnobserveAsync(ElementReference)
        Mock.Setup(x => x.UnobserveAsync(It.IsAny<ElementReference>()))
            .Returns(Task.CompletedTask)
            .Callback<ElementReference>(elementRef => _callbacks.Remove($"element-{elementRef.Id}"));

        // Setup DisposeAsync
        Mock.Setup(x => x.DisposeAsync())
            .Returns(ValueTask.CompletedTask)
            .Callback(() => _callbacks.Clear());
    }

    /// <summary>
    ///     Simulates a resize event for the specified element.
    /// </summary>
    public void TriggerResize(string elementId, ResizeObserverEntry entry)
    {
        if (_callbacks.TryGetValue(elementId, out var callback))
        {
            callback(entry);
        }
    }

    /// <summary>
    ///     Checks if an element is currently being observed.
    /// </summary>
    public bool IsObserving(string elementId) => _callbacks.ContainsKey(elementId);

    /// <summary>
    ///     Gets the count of currently observed elements.
    /// </summary>
    public int ObservedCount => _callbacks.Count;

    /// <summary>
    ///     Gets all observed element IDs.
    /// </summary>
    public IEnumerable<string> ObservedElements => _callbacks.Keys;
}