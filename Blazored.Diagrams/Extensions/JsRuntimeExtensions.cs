using System.Diagnostics.CodeAnalysis;
using Blazored.Diagrams.Components.Containers;
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
    /// <param name="jsRuntime"><see cref="IJSRuntime"/>.</param>
    /// <param name="element">Reference to the desired element.</param>
    /// <returns></returns>
    public static async Task<Rect> GetBoundingClientRect(this IJSRuntime jsRuntime, ElementReference element)
    {
        var result = await jsRuntime.InvokeAsync<Rect>(JsFunctionConstants.BoundingClientRectFunctionName, element);
        return result;
    }

    /// <summary>
    /// Check if a click was on a svg path.
    /// </summary>
    /// <param name="jsRuntime"><see cref="IJSRuntime"/>.</param>
    /// <param name="args">Mouse event arguments from the click.</param>
    /// <param name="pathId">Id of the path</param>
    /// <returns></returns>
    public static async Task<bool> IsClickOnPath(this IJSRuntime jsRuntime, MouseEventArgs args, string pathId)
    {
        return await jsRuntime.InvokeAsync<bool>(JsFunctionConstants.IsClickOnPathFunctionName, pathId, args.ClientX,
            args.ClientY);
    }

    /// <summary>
    ///  Prevents scrolling when focused on an element.
    /// </summary>
    /// <param name="jsRuntime"><see cref="IJSRuntime"/>.</param>
    /// <param name="containerId">ID of the container element</param>
    /// <param name="reference">Reference to the container where the wheel event occured.</param>
    public static async Task HandleWheelEvent(this IJSRuntime jsRuntime, string containerId,
        DotNetObjectReference<DiagramContainer> reference)
    {
        await jsRuntime.InvokeVoidAsync(JsFunctionConstants.HandleZoomFunctionName, containerId, reference);
    }
}