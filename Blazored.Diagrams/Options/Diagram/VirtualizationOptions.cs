namespace Blazored.Diagrams.Options.Diagram;

/// <summary>
/// Virtualization options.
/// </summary>
public class VirtualizationOptions
{
    /// <summary>
    /// Enables/disables virtualization.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Space in pixels to make model outside the viewport disappear.
    /// </summary>
    public int BufferSize { get; set; } = 200;
}