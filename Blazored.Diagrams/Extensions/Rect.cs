using System.Diagnostics.CodeAnalysis;

namespace Blazored.Diagrams.Extensions;

/// <summary>
///     Helper class for GetBoundClientRect
/// </summary>
[ExcludeFromCodeCoverage]
public class Rect
{
    /// <summary>
    /// Width of the rectangle
    /// </summary>
    public double Width { get; init; }
        
    /// <summary>
    /// Height of the rectangle
    /// </summary>
    public double Height { get; init; }
    /// <summary>
    /// Top coordinate of the rectangle
    /// </summary>
    public double Top { get; init; }
    /// <summary>
    /// Right coordinate of the rectangle
    /// </summary>
    public double Right { get; init; }
    /// <summary>
    /// Bottom coordinate of the rectangle
    /// </summary>
    public double Bottom { get; init; }
    /// <summary>
    /// Left coordinate of the rectangle
    /// </summary>
    public double Left { get; init; }
}