namespace Blazored.Diagrams.Extensions;

/// <summary>
/// Names of methods used by JS interop
/// </summary>
public static class JsFunctionConstants
{
    /// <summary>
    /// Function that initializes a resize observer
    /// </summary>
    public const string InitializeResizeObserver = "BlazoredDiagrams.initializeResizeObserver";

    /// <summary>
    /// Function that gets an element by its id.
    /// </summary>
    public const string GetElementId = "BlazoredDiagrams.getElementId";

    /// <summary>
    /// Function that unregisters all observers.
    /// </summary>
    public const string RemoveAllObservers = "BlazoredDiagrams.removeAllResizeObservers";

    /// <summary>
    /// Function that removes an observer based on the html element id.
    /// </summary>
    public const string UnregisterObserver = "BlazoredDiagrams.removeResizeObserver";

    /// <summary>
    /// Name of the js function that gets a rectangle for an element.
    /// </summary>
    public const string BoundingClientRectFunctionName = "BlazoredDiagrams.getBoundingClientRect";

    /// <summary>
    /// Name of the js function that checks if a mouse click hits a svg path.
    /// </summary>
    public const string IsClickOnPathFunctionName = "BlazoredDiagrams.isClickOnPath";

    /// <summary>
    /// Name of the js function that handles zoom event properly.
    /// </summary>
    public const string CaptureComponentAsDataUrl = "BlazoredDiagrams.captureComponentAsDataUrl";
    public const string DownloadDataUrl = "BlazoredDiagrams.downloadDataUrl";
    public const string CaptureAndDownload = "BlazoredDiagrams.captureAndDownload";
}