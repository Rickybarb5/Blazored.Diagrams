using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Components.Models;
using Blazored.Diagrams.Links;

namespace Blazored.Diagrams.Options.Behaviours;

/// <summary>
/// Options for <see cref="DrawLinkBehavior"/>
/// </summary>
public class DrawLinkOptions : BehaviourOptionsBase
{
    /// <summary>
    ///     Type of link that the behaviour will create.
    /// Default is <see cref="Link"/>
    /// </summary>
    public Type LinkType = typeof(Link);

    /// <summary>
    ///     Color of the link path. Default is black.
    /// </summary>
    public string StrokeColor { get; set; } = "black";

    /// <summary>
    /// Color of the link path when it's selected. Default is lightyellow.
    /// </summary>
    public string SelectedStrokeColor { get; set; } = "lightyellow";

    /// <summary>
    ///     Width of the path. Default is 4.
    /// </summary>
    public int StrokeWidth { get; set; } = 4;

    /// <summary>
    ///     Default link type when using the <see cref="DefaultLinkComponent"/>.
    ///     Will only be applied to newly created links.
    /// </summary>
    public LinkPath DefaultLinkPath { get; set; }
}

/// <summary>
///     How the <see cref="DefaultLinkComponent"/> is drawn.
/// </summary>
public enum LinkPath
{
    /// <summary>
    /// Curved link.
    /// </summary>
    Curved,

    /// <summary>
    /// Straight line.
    /// </summary>
    Line,

    /// <summary>
    /// Orthogonal line
    /// </summary>
    Orthogonal,
}