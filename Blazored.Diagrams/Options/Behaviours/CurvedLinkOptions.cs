using Blazored.Diagrams.Components.Models;

namespace Blazored.Diagrams.Options.Behaviours;

/// <summary>
/// Options for <see cref="CurvedLinkComponent"/>
/// </summary>
public class CurvedLinkOptions : BaseLinkOptions
{
    /// <summary>
    ///     Wave height.
    /// </summary>
    public int WaveHeight { get; set; } = 20;
}