using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Blazored.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazored.Diagrams.Services.Observers;

/// <summary>
///     Service that provides methods to observe size changes of a html element.
/// </summary>
[ExcludeFromCodeCoverage]
public class ResizeObserverService : IResizeObserverService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ConcurrentDictionary<string, Action<ResizeObserverEntry>> _callbacks = new();
    private readonly DotNetObjectReference<ResizeObserverService> _dotNetRef;

    /// <summary>
    /// Instantiates a new <see cref="ResizeObserverService"/>
    /// </summary>
    /// <param name="jsRuntime"></param>
    public ResizeObserverService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _dotNetRef = DotNetObjectReference.Create(this);
    }

    /// <inheritdoc />
    public async Task ObserveAsync(string elementId, Action<ResizeObserverEntry> callback)
    {
        _callbacks[elementId] = callback;
        await _jsRuntime.InvokeVoidAsync(JsFunctionConstants.InitializeResizeObserver, _dotNetRef, elementId);
    }

    /// <inheritdoc />
    public async Task ObserveAsync(ElementReference elementRef, Action<ResizeObserverEntry> callback)
    {
        var elementId = await _jsRuntime.InvokeAsync<string>(JsFunctionConstants.GetElementId, elementRef);
        await ObserveAsync(elementId, callback);
    }

    /// <inheritdoc />
    public async Task UnobserveAsync(string elementId)
    {
        await _jsRuntime.InvokeVoidAsync(JsFunctionConstants.UnregisterObserver, elementId);
        _callbacks.TryRemove(elementId, out _);
    }

    /// <inheritdoc />
    public async Task UnobserveAsync(ElementReference elementRef)
    {
        var elementId = await _jsRuntime.InvokeAsync<string>(JsFunctionConstants.GetElementId, elementRef);
        await UnobserveAsync(elementId);
    }

    /// <inheritdoc />
    [JSInvokable]
    public void OnResizeAsync(string elementId, ResizeObserverEntry entry)
    {
        if (_callbacks.TryGetValue(elementId, out var callback))
        {
            callback(entry);
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await _jsRuntime.InvokeVoidAsync(JsFunctionConstants.RemoveAllObservers);
        _callbacks.Clear();
    }
}

/// <summary>
/// 
/// </summary>
[ExcludeFromCodeCoverage]
public record ResizeObserverEntry
{
    /// <summary>
    /// New width of the element
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// New height of the element
    /// </summary>
    public double Height { get; set; }
}