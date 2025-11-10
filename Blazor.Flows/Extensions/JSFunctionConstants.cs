namespace Blazor.Flows.Extensions;

/// <summary>
/// Names of methods used by JS interop
/// </summary>
public static class JsFunctionConstants
{
    /// <summary>
    /// Function that initializes a resize observer
    /// </summary>
    public const string InitializeResizeObserver = "BlazorFlows.initializeResizeObserver";

    /// <summary>
    /// Function that gets an element by its id.
    /// </summary>
    public const string GetElementId = "BlazorFlows.getElementId";

    /// <summary>
    /// Function that unregisters all observers.
    /// </summary>
    public const string RemoveAllObservers = "BlazorFlows.removeAllResizeObservers";

    /// <summary>
    /// Function that removes an observer based on the html element id.
    /// </summary>
    public const string UnregisterObserver = "BlazorFlows.removeResizeObserver";

    /// <summary>
    /// Name of the js function that gets a rectangle for an element.
    /// </summary>
    public const string BoundingClientRectFunctionName = "BlazorFlows.getBoundingClientRect";

    /// <summary>
    /// Name of the js function that checks if a mouse click hits a svg path.
    /// </summary>
    public const string IsClickOnPathFunctionName = "BlazorFlows.isClickOnPath";

    /// <summary>
    /// Name of the js function that handles zoom event properly.
    /// </summary>
    public const string HandleZoomFunctionName = "BlazorFlows.handleZoom";
}