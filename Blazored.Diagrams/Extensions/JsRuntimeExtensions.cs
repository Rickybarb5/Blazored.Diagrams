using System.Diagnostics.CodeAnalysis;
using Blazored.Diagrams.Components.Containers;
using Blazored.Diagrams.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blazored.Diagrams.Extensions;

/// <summary>
///     Wrapper class to call js methods
/// </summary>
[ExcludeFromCodeCoverage]
public static class JsRuntimeExtensions
{
    /// <summary>
    ///     Fetches the size of a html element.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="element"></param>
    /// <returns></returns>
    public static async Task<Rect> GetBoundingClientRect(this IJSRuntime jsRuntime, ElementReference element)
    {
        var result = await jsRuntime.InvokeAsync<Rect>(JsFunctionConstants.BoundingClientRectFunctionName, element);
        return result;
    }

    /// <summary>
    /// Check if a click was on a svg path
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="e"></param>
    /// <param name="pathId"></param>
    /// <returns></returns>
    public static async Task<bool> IsClickOnPath(this IJSRuntime jsRuntime, MouseEventArgs e, string pathId)
    {
        return await jsRuntime.InvokeAsync<bool>(JsFunctionConstants.IsClickOnPathFunctionName, pathId, e.ClientX,
            e.ClientY);
    }

    /// <summary>
    ///  Prevents scrolling when focused on an element.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="containerId"></param>
    /// <param name="reference"></param>
    public static async Task HandleWheelEvent(this IJSRuntime jsRuntime, string containerId,
        DotNetObjectReference<DiagramContainer> reference)
    {
        await jsRuntime.InvokeVoidAsync("BlazoredDiagrams.handleZoom", containerId, reference);
    }
}