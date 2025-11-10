namespace Blazor.Flows.Options.Behaviours;

/// <summary>
/// Basic options for links.
/// </summary>
public class BaseLinkOptions
{
    /// <summary>
    /// Color of the link path. Default is black.
    /// </summary>
    public string StrokeColor { get; set; } = "black";

    /// <summary>
    /// Color of the link path when it's selected. Default is gray.
    /// </summary>
    public string SelectedStrokeColor { get; set; } = "gray";

    /// <summary>
    /// Width of the path. Default is 4.
    /// </summary>
    public int StrokeWidth { get; set; } = 4;
}